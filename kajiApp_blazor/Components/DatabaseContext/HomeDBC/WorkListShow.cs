using kajiApp_blazor.Components.Models.HomeModel;
using Microsoft.Data.Sqlite;

namespace kajiApp_blazor.Components.DatabaseContext.HomeDBC

{
    public  class WorkListShow
    {
        private  readonly string _connectionString = "Data Source=database.db";
        //非同期版
        public  async Task<List<WorkList>> GetWorksAsync() // ✅ 非同期メソッド
        {
            await Task.Delay(1000);
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync(); // ✅ OpenAsync() を使う

            var command = connection.CreateCommand();
            command.CommandText = "SELECT id, day, name, work, percent FROM works " +
                                  "WHERE day BETWEEN DATE('now', '-5 day') " +
                                  "AND DATE('now') ORDER BY id DESC LIMIT 15";

            var worklist = new List<WorkList>();
            //sqliteのExecuteReaderAsyncは1明細ずつ取得する仕組みらしい
            using var reader = await command.ExecuteReaderAsync(); // ✅ 非同期実行
            //データをある分だけ1明細ずつ取得 
            while (await reader.ReadAsync()) // ✅ 非同期読み取り
            {
                worklist.Add(new WorkList
                {
                    Id = reader.GetInt32(0),
                    Day = reader.GetDateTime(1),
                    Name = reader.GetString(2),
                    WorkName = reader.GetString(3),
                    Percent = reader.GetDouble(4)
                });
            }
            return worklist; // ✅ 戻り値を Task<List<Work>> にする
        }
    }
}


