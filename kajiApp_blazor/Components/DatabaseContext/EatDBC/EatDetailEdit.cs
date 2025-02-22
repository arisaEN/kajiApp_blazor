using System.Collections.Generic;
using System.Diagnostics;
using kajiApp_blazor.Components.Models.EatModel;
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
        public async Task InsertEatDetailAsync(int year, int month, decimal amount)
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
    }
}
