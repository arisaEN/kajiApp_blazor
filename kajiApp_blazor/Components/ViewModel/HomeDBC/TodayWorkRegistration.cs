using kajiApp_blazor.Components.DTO.HomeModel;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using kajiApp_blazor.Components.Entity;
using Microsoft.EntityFrameworkCore;

namespace kajiApp_blazor.Components.ViewModel.HomeDBC
{


    /// <summary>
    /// 登録画面を作る
    /// </summary>
    public class TodayWorkRegistration
    {
        private readonly string _connectionString = "Data Source=database.db";
        private readonly kajiappDBContext _context;

        public TodayWork FormModel { get; private set; } = new TodayWork();

        // 名前リスト作成（DBから取得）
        public List<string> NameList { get; private set; } = new();

        // 仕事リスト作成（DBから取得）
        public List<WorkItem> WorkList { get; private set; } = new();

        /// <summary>
        /// コンストラクタでDBからリストに値を入れて親に返す
        /// </summary>
        public TodayWorkRegistration(kajiappDBContext context)
        {
            _context = context;
            LoadDataFromDatabase();
        }

        /// <summary>
        /// 入力フォームを作る
        /// </summary>
        private void LoadDataFromDatabase()
        {
            //名前リスト
            // ① Entity を取得
            var nameLists =  _context.NameLists
                .Select(n => new
                {
                    n.NameId,
                    n.Name 
                })
                .ToList();
            // ② NameList に追加
            NameList.AddRange(nameLists.Select(n => n.Name));

           // //家事リスト
           // // ① Entity を取得
           // var workLists = _context.WorkLists
           //     .Select(w => new
           //     {
           //         w.WorkId,
           //         w.WorkName
           //     })
           //     .ToList();
           // // ② NameList に追加
           //WorkList.AddRange(workLists.Select(w => w.WorkName));

            //using (var connection = new SQLiteConnection(_connectionString))
            //{
            //    connection.Open();

            //    // NameListの取得
            //    using (var command = new SQLiteCommand("SELECT name FROM nameList", connection))
            //    using (var reader = command.ExecuteReader())
            //    {
            //        while (reader.Read())
            //        {
            //            NameList.Add(reader.GetString(0));
            //        }
            //    }

            // WorkListの取得
            //using (var command = new SQLiteCommand("SELECT work_id, workName FROM workList", connection))
            //using (var reader = command.ExecuteReader())
            //{
            //    while (reader.Read())
            //    {
            //        WorkList.Add(new WorkItem(reader.GetInt32(0), reader.GetString(1)));
            //    }
            //}
            //}
        }



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
            try
            {
                using var connection = new SQLiteConnection(_connectionString);
                await connection.OpenAsync();

                using var command = new SQLiteCommand(
                    "INSERT INTO works (day, name, work_id, work, percent) VALUES (@day, @name, @work_id, @work_name, @percent)",
                    connection);
                command.Parameters.AddWithValue("@day", FormModel.Day.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@name", FormModel.Name);
                command.Parameters.AddWithValue("@work_id", FormModel.WorkId);
                command.Parameters.AddWithValue("@work_name", FormModel.WorkName);
                command.Parameters.AddWithValue("@percent", FormModel.Percent);

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