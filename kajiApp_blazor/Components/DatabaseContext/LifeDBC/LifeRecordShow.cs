using System.Collections.Generic;
using System.Diagnostics;
using kajiApp_blazor.Components.Models.EatModel;
using kajiApp_blazor.Components.Models.LifeModel;
using Microsoft.Data.Sqlite;

namespace kajiApp_blazor.Components.DatabaseContext.LifeDBC
{
    public class LifeRecordShow
    {
        private readonly string _connectionString = "Data Source=database.db";
        //非同期版
        public async Task<List<LifeRecord>> GetLifeAsync() // ✅ 非同期メソッド
        {
            //await Task.Delay(1000);
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync(); // ✅ OpenAsync() を使う

            var command = connection.CreateCommand();
            command.CommandText = "SELECT year, month, rent, water, electricity, gas " +
                                                    "FROM life_detail " +
                                                    "ORDER BY yyyymm desc ";

            var eatrecord = new List<LifeRecord>();
            //sqliteのExecuteReaderAsyncは1明細ずつ取得する仕組みらしい
            using var reader = await command.ExecuteReaderAsync(); // ✅ 非同期実行
            //データをある分だけ1明細ずつ取得 
            while (await reader.ReadAsync()) // ✅ 非同期読み取り
            {
                eatrecord.Add(new LifeRecord
                {
                    year = reader.GetInt32(0),
                    month = reader.GetInt32(1),
                    rent = reader.GetInt32(2),
                    water = reader.GetInt32(3),
                    electricity = reader.GetInt32(4),
                    gas = reader.GetInt32(5)
                });
            }
            return eatrecord; // ✅ 戻り値を Task<List<Work>> にする
        }
    }
}
