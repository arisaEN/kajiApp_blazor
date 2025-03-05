using System.Collections.Generic;
using System.Diagnostics;
using kajiApp_blazor.Components.DataModels.EatModel;
using Microsoft.Data.Sqlite;

namespace kajiApp_blazor.Components.DatabaseContext.EatDBC

{
    public class EatRecordShow
    {
        private readonly string _connectionString = "Data Source=database.db";
        //非同期版
        public async Task<List<EatRecord>> GetEatAsync() // ✅ 非同期メソッド
        {
            //await Task.Delay(1000);
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync(); // ✅ OpenAsync() を使う

            var command = connection.CreateCommand();
            command.CommandText = "SELECT year, month, amount "+
                                                    "FROM eat "+
                                                    "ORDER BY yyyymm DESC "+
                                                    "LIMIT 12 ";

            var eatrecord = new List<EatRecord>();
            //sqliteのExecuteReaderAsyncは1明細ずつ取得する仕組みらしい
            using var reader = await command.ExecuteReaderAsync(); // ✅ 非同期実行
            //データをある分だけ1明細ずつ取得 
            while (await reader.ReadAsync()) // ✅ 非同期読み取り
            {
                eatrecord.Add(new EatRecord
                {
                    Year = reader.GetString(0),
                    Month = reader.GetString(1),
                    Amount = reader.GetInt32(2)
                });
            }
            return eatrecord; // ✅ 戻り値を Task<List<Work>> にする
        }
    }
}