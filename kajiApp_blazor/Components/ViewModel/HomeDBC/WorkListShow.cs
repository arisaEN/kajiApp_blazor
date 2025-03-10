using System.Text.RegularExpressions;
using System.Xml.Linq;
using kajiApp_blazor.Components.DTO.HomeModel;
using Microsoft.Data.Sqlite;
using kajiApp_blazor.Components.DTO;
using Microsoft.EntityFrameworkCore;
using kajiApp_blazor.Components.Entity;


namespace kajiApp_blazor.Components.ViewModel.HomeDBC

{
    public class WorkListShow
    {
        //private readonly string _connectionString = "Data Source=database.db";
        private readonly kajiappDBContext _context;

        public WorkListShow(kajiappDBContext context)
        {
            _context = context;
        }
        public async Task<List<DTO.HomeModel.WorkList>> GetWorksAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var fromDate = today.AddDays(-5);

            // まずデータベースからリストを取得（この時点では SQL クエ
            // リの範囲）
            var worklist = await _context.Works
                .Where(w => w.Day >= fromDate && w.Day <= today)
                .OrderByDescending(w => w.Id)
                .Take(15)
                .ToListAsync(); // ← ここでメモリに読み込む

            // メモリ上で変換処理を行う（式ツリーの制約を回避）
            var result = worklist.Select(w => new DTO.HomeModel.WorkList
            {
                Id = w.Id,
                Day = w.Day.HasValue ? w.Day.Value.ToDateTime(TimeOnly.MinValue) : DateTime.MinValue,
                Name = w.Name ?? "",
                WorkName = w.Work1 ?? "",
                Percent = int.TryParse(w.Percent, out var percent) ? percent : 0
            }).ToList(); // ← 最終的なリストを作成

            return result;
        }
    }
}


