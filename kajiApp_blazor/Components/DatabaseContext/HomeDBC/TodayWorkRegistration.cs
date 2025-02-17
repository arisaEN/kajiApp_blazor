using kajiApp_blazor.Components.Models.HomeModel;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace kajiApp_blazor.Components.Data.HomeData
{


    /// <summary>
    /// 登録画面を作る
    /// </summary>
    public class TodayWorkRegistration
    {
        private const string DatabaseFile = "database.db";
        private static readonly string ConnectionString = "Data Source=" + DatabaseFile + ";";
        private static readonly string _connectionString = "Data Source=database.db";  // DB接続文字列

        //インスタンス生成ででフォームに使うNameListとWorkList作成
        //2025.2.16 ダミーデータを使用。ほんとはDBからデータを持ってくる。
        public TodayWork FormModel { get; private set; } = new TodayWork();
        //名前リスト作成
        public List<string> NameList { get; } = new() { "田中", "佐藤", "鈴木" };


        //public static async Task<List<string>> GetNameListAsync()
        //{
        //    await Task.Delay(2000);  // 例として遅延を追加
        //    using var connection = new SQLiteConnection(_connectionString);
        //    await connection.OpenAsync(); // 非同期で接続

        //    var command = connection.CreateCommand();
        //    command.CommandText = "SELECT name FROM nameList"; // SQLクエリ

        //    var nameList = new List<string>();

        //    using var reader = await command.ExecuteReaderAsync(); // 非同期で読み取る
        //    while (await reader.ReadAsync()) // 非同期で1行ずつ読み取る
        //    {
        //        nameList.Add(reader.GetString(0));  // 名前をリストに追加
        //    }

        //    return nameList;  // 非同期メソッドとしてリストを返す
        //}







        //仕事リスト作成
        public List<WorkItem> WorkList { get; } = new List<WorkItem>()
        {
            new(1, "掃除"),
            new(2, "洗濯"),
            new(3, "料理")
        };
         
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public void UpdateWorkName(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int workId))
            {
                var selectedWork = WorkList.FirstOrDefault(w => w.Id == workId);
                FormModel.WorkName = selectedWork?.Name ?? "";
            }
        }

        /// <summary>
        /// 登録処理
        /// </summary>
        /// <returns></returns>
        public async Task SaveToDatabaseAsync()
        {
            await using var connection = new SqliteConnection(ConnectionString);
            await connection.OpenAsync();

            string insertQuery = @"
            INSERT INTO works (work_id, work_name, day, name, percent)
            VALUES (@work_id, @work_name, @day, @name, @percent);";

            await using var command = new SqliteCommand(insertQuery, connection);
            command.Parameters.AddWithValue("@work_id", FormModel.WorkId);
            command.Parameters.AddWithValue("@work_name", FormModel.WorkName);
            command.Parameters.AddWithValue("@day", FormModel.Day.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@name", FormModel.Name);
            command.Parameters.AddWithValue("@percent", FormModel.Percent);

            await command.ExecuteNonQueryAsync();
        }
    }

   
}
