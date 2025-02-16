using kajiApp_blazor.Components.Models.HomeModel;
using Microsoft.Data.Sqlite;

namespace kajiApp_blazor.Components.Data.HomeData

{
    public static class WorkListShow
    {
        private static readonly string _connectionString = "Data Source=database.db";

        //非同期版
        public static async Task<List<Work>> GetWorksAsync() // ✅ 非同期メソッド
        {
            await Task.Delay(2000);
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync(); // ✅ OpenAsync() を使う

            var command = connection.CreateCommand();
            command.CommandText = "SELECT id, day, name, work, percent FROM works " +
                                  "WHERE day BETWEEN DATE('now', '-1 day') " +
                                  "AND DATE('now') ORDER BY id DESC LIMIT 15";

            var works = new List<Work>();
            //sqliteのExecuteReaderAsyncは1明細ずつ取得する仕組みらしい
            using var reader = await command.ExecuteReaderAsync(); // ✅ 非同期実行
            //データをある分だけ1明細ずつ取得 
            while (await reader.ReadAsync()) // ✅ 非同期読み取り
            {
                works.Add(new Work
                {
                    Id = reader.GetInt32(0),
                    Day = reader.GetDateTime(1),
                    Name = reader.GetString(2),
                    WorkName = reader.GetString(3),
                    Percent = reader.GetDouble(4)
                });
            }
            return works; // ✅ 戻り値を Task<List<Work>> にする
        }

        // 他のデータ取得メソッドも同様に実装
    }
}
