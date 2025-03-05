using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using kajiApp_blazor.Components.DataModels.EatModel;
using Microsoft.Data.Sqlite;

namespace kajiApp_blazor.Components.DatabaseContext.EatDBC
{
    public class EatDetailEdit
    {
        private readonly string _connectionString = "Data Source=database.db";

        /// <summary>
        /// 明細を保存
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public async Task InsertEatDetailAsync(string year, string month, decimal? amount)
        {
            const string query = "INSERT INTO eat_detail (year, month, amount) VALUES (@year, @month, @amount)";

            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@year", year);
            command.Parameters.AddWithValue("@month", month);
            command.Parameters.AddWithValue("@amount", amount);

            await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// detail明細の合計をeatテーブルに反映
        /// </summary>
        /// <returns></returns>
        public async Task EatDetailSumAsync()
        {
            const string query = @"UPDATE eat 
                                  SET amount = (
                                    SELECT COALESCE(SUM(ed.amount), 0) 
                                    FROM eat_detail ed 
                                    WHERE ed.year = eat.year AND ed.month = eat.month
                                  )";

            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = query;

            await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// 明細一覧取得
        /// </summary>
        /// <returns></returns>
        public async Task<List<EatDetailRecord>> GetEatDetailAsync(string year, string month)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync(); // ✅ OpenAsync() を使う

            var command = connection.CreateCommand();
            command.CommandText = "SELECT id, amount, input_time " +
                                  "FROM eat_detail " +
                                  "WHERE year = @year AND month = @month "+
                                  "ORDER BY id desc ";// ✅ パラメータ化クエリ

            command.Parameters.AddWithValue("@year", year);
            command.Parameters.AddWithValue("@month", month);

            var eatDetailRecord = new List<EatDetailRecord>();

            using var reader = await command.ExecuteReaderAsync(); // ✅ 非同期実行
            while (await reader.ReadAsync()) // ✅ 非同期読み取り
            {
                eatDetailRecord.Add(new EatDetailRecord
                {
                    Id = reader.GetInt32(0),
                    Amount = reader.GetInt32(1),
                    Yyyymm = reader.GetString(2) // 日時型のカラムなら GetDateTime()
                });
            }

            return eatDetailRecord;
        }

        /// <summary>
        /// 明細アップデート
        /// </summary>
        /// <param name="id"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public async Task UpdateEatDetailAsync(int id, int amount)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();
            var command = connection.CreateCommand();
            command.CommandText = "UPDATE eat_detail " +
                                                    "SET amount = @amount " +
                                                    "WHERE id = @id";
            command.Parameters.AddWithValue("@amount", amount);
            command.Parameters.AddWithValue("@id", id);
            await command.ExecuteNonQueryAsync();
        }
    }
}
