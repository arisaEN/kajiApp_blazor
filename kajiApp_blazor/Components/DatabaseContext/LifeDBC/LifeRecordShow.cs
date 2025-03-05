using System.Collections.Generic;
using System.Diagnostics;
using kajiApp_blazor.Components.DataModels.EatModel;
using kajiApp_blazor.Components.DataModels.LifeModel;
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
            command.CommandText = "SELECT id , year, month, rent, water, electricity, gas " +
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
                    Id = reader.GetInt32(0),
                    Year = reader.GetString(1),
                    Month = reader.GetString(2),
                    Rent = reader.GetInt32(3),
                    Water = reader.GetInt32(4),
                    Electricity = reader.GetInt32(5),
                    Gas = reader.GetInt32(6)
                });
            }
            return eatrecord; // ✅ 戻り値を Task<List<Work>> にする
        }
        
        

    }
}
