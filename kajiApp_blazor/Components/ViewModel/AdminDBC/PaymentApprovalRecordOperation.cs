using kajiApp_blazor.Components.DTO.AdminModel;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using kajiApp_blazor.Components.Entity;


namespace kajiApp_blazor.Components.ViewModel.AdminDBC
{
    public class PaymentApprovalRecordOperation
    {
        private readonly kajiappDBContext _context;
        public PaymentApprovalRecordOperation(kajiappDBContext context)
        {
            _context = context;
        }

        public async Task<List<PaymentApprovalRecord>> GetPaymentApprovalRecordAsync()
        {
            try
            {
                // ① Entity を取得
                var payrecord = await _context.Payments
                    .Join(
                        _context.NameLists,
                        payment => payment.NameCode,  // 支払いテーブルの結合キー
                        nameList => nameList.NameId, // 名前リストの結合キー
                        (payment, nameList) => new  // 結合結果（Entity のまま）
                        {
                            payment.Yyyymm,
                            nameList.Name,
                            payment.Pay,
                            payment.決裁,
                            payment.Id
                        }
                    )
                    .Where(p => p.決裁 != "済" || string.IsNullOrEmpty(p.決裁)) // 条件フィルタ
                    .OrderBy(p => p.Yyyymm)
                    .ToListAsync();

                // ② DTO に変換
                var paymentApprovalRecord = payrecord.Select(p => new PaymentApprovalRecord
                {
                    YearMonth = p.Yyyymm,
                    Payer = p.Name,
                    Payment = p.Pay ?? 0, // NULL の場合は 0
                    Status = p.決裁 ?? string.Empty, // NULL の場合は 空文字
                    Id = p.Id
                }).ToList();

                return paymentApprovalRecord; // DTO を返す
            }
            catch (Exception ex)
            {
                Console.WriteLine($"エラー: {ex.Message}");
                Console.WriteLine($"スタックトレース: {ex.StackTrace}");
                return new List<PaymentApprovalRecord>(); // エラー時は空リストを返す
            }
        }
        /// <summary>
        /// admin支払金額更新
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> EditPaymentAsync(string YearMonth, int Payment, string Status)
        {
            try
            {
                //引数Yyyymmのレコードを取得 レコードは主キーなので1件しか存在しない想定
                var payment = await _context.Payments
                                           .FirstOrDefaultAsync(p => p.Yyyymm == YearMonth);

                if (payment == null)
                {
                    return false; // 該当レコードなし
                }

                payment.Pay = Payment;
                payment.決裁 = Status;

                await _context.SaveChangesAsync(); // 更新をデータベースに適用
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"データ更新中にエラーが発生しました: {ex.Message}");
                return false;
            }
        }
    }
}