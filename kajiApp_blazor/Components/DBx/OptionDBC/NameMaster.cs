using System.Data.SQLite;
using kajiApp_blazor.Components.DataModels.OptionModel;
using Microsoft.Data.Sqlite;


namespace kajiApp_blazor.Components.DBx.OptionDBC
{
    public class NameMaster
    {
        private readonly string _connectionString = "Data Source=database.db";
        //非同期版
        public async Task<List<NameMasterList>> GetNameMasterAsync() // ✅ 非同期メソッド
        {
            //await Task.Delay(1000);
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync(); // ✅ OpenAsync() を使う

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * " +
                                                    "FROM nameList " +
                                                    "ORDER BY name_id desc";

            var nameMasterList = new List<NameMasterList>();
            //sqliteのExecuteReaderAsyncは1明細ずつ取得する仕組みらしい
            using var reader = await command.ExecuteReaderAsync(); // ✅ 非同期実行
            //データをある分だけ1明細ずつ取得 
            while (await reader.ReadAsync()) // ✅ 非同期読み取り
            {
                nameMasterList.Add(new NameMasterList
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1)
                });
            }
            return nameMasterList; // ✅ 戻り値を Task<List<Work>> にする
        }

        public async Task InsertNameMasterAsync(NameMasterList newName)
        {
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO nameList (name) " +
                                                        "VALUES (@Name) ";

                command.Parameters.AddWithValue("@Name", newName.Name);


                await command.ExecuteNonQueryAsync();

                Console.WriteLine("データを登録しました");
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"SQLite エラー: {ex.Message}");
                Console.WriteLine($"スタックトレース: {ex.StackTrace}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"予期しないエラー: {ex.Message}");
                Console.WriteLine($"スタックトレース: {ex.StackTrace}");
            }
        }
    }
}
