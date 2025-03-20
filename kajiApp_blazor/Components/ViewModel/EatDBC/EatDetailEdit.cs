using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using kajiApp_blazor.Components.DTO.AdminModel;
using kajiApp_blazor.Components.DTO.EatModel;
using kajiApp_blazor.Domain.Entity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;


namespace kajiApp_blazor.Components.ViewModel.EatDBC
{
    public class EatDetailEdit
    {
        private readonly kajiappDBContext _context;
        public EatDetailEdit(kajiappDBContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 明細を保存
        /// </summary>
        public async Task InsertEatDetailAsync(string year, string month, int? amount)
        {
            //Entityモデルをnew 引数を代入する。
            var eatDetail = new EatDetail
            {
                Year = year,
                Month = month,
                Amount = amount,
                InputTime = DateTime.UtcNow
            };
            //レコード追加とDB保存
            await _context.EatDetails.AddAsync(eatDetail);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// detail明細の合計をeatテーブルに反映
        /// </summary>
        /// <returns>これ全件更新しているけど、呼び出し元からyyyymmもらって対象の月のみ更新にしたほうがいい</returns>
        public async Task EatDetailSumAsync()
        {
            var eatDetailSums = await _context.EatDetails
                .GroupBy(ed => new { ed.Year, ed.Month })
                .Select(g => new { g.Key.Year, g.Key.Month, TotalAmount = g.Sum(ed => ed.Amount ?? 0) })
                .ToListAsync();
            //Eatレコードを更新
            foreach (var sum in eatDetailSums)
            {
                var eat = await _context.Eats
                    .FirstOrDefaultAsync(e => e.Year == sum.Year && e.Month == sum.Month);
                //あれば更新
                if (eat != null)
                {
                    eat.Amount = sum.TotalAmount;
                }
                //なければ新規明細作成
                else
                {
                    _context.Eats.Add(new Eat
                    {
                        Year = sum.Year,
                        Month = sum.Month,
                        Amount = sum.TotalAmount,
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 明細一覧取得
        /// </summary>
        /// <returns></returns>
        public async Task<List<EatDetailRecord>> GetEatDetailAsync(string year, string month)
        {
            // ① Entity を取得
            var eatDetails = await _context.EatDetails
                .Where(ed => ed.Year == year && ed.Month == month)
                .OrderByDescending(ed => ed.Id)
                .Select(ed => new
                {
                    ed.Id,
                    Amount = ed.Amount ?? 0, // Nullable int を int に変換
                    InputTime = ed.InputTime.HasValue ? ed.InputTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : null // Nullable DateTime を string に変換
                })
                .ToListAsync();

            // ② DTO に変換
            var eatDetailRecord = eatDetails.Select(ed => new EatDetailRecord
            {
                Id = ed.Id,
                Amount = ed.Amount,
                Yyyymm = ed.InputTime 
            }).ToList();

            return eatDetailRecord; // DTO を返す
        }
        /// <summary>
        /// 明細アップデート
        /// </summary>
        /// <param name="id"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public async Task UpdateEatDetailAsync(int id, int amount)
        {
            //引数idのレコードを取得 レコードは主キーなので1件しか存在しない想定
            var eatDetail = await _context.EatDetails
                                       .FirstOrDefaultAsync(e => e.Id == id);
            if (eatDetail == null)
            {
                 // 該当レコードなし
            }
            eatDetail.Amount = amount;
           
            await _context.SaveChangesAsync(); // 更新をデータベースに適用
            
            //using var connection = new SqliteConnection(_connectionString);
            //await connection.OpenAsync();
            //var command = connection.CreateCommand();
            //command.CommandText = "UPDATE eat_detail " +
            //                                        "SET amount = @amount " +
            //                                        "WHERE id = @id";
            //command.Parameters.AddWithValue("@amount", amount);
            //command.Parameters.AddWithValue("@id", id);
            //await command.ExecuteNonQueryAsync();
        }
    }
}
