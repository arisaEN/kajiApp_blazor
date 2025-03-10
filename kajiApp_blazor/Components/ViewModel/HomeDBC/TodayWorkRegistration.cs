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
using AngleSharp.Diffing.Extensions;

namespace kajiApp_blazor.Components.ViewModel.HomeDBC
{
    /// <summary>
    /// 登録画面を作る
    /// </summary>
    public class TodayWorkRegistration
    {
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
                .Select(n => n.Name)
                .ToList();
            NameList.AddRange(nameLists);

            //家事リスト
            // ① Entity を取得
            var workLists = _context.WorkLists
            .Select(w => new WorkItem(w.WorkId, w.WorkName))
            .ToList();
            WorkList.AddRange(workLists);
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
                var work = new Work
                {
                    Day = DateOnly.FromDateTime(FormModel.Day),
                    Name = FormModel.Name,
                    WorkId = FormModel.WorkId,
                    Work1 = FormModel.WorkName, // WorkName → Work1 にマッピング
                    Percent = FormModel.Percent.ToString()
                };

                await _context.Works.AddAsync(work);
                await _context.SaveChangesAsync();

                Console.WriteLine("データを登録しました");
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"データベース更新エラー: {ex.Message}");
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