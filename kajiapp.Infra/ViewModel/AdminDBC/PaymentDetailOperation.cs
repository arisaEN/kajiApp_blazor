using kajiApp_blazor.Infra.DTO.AdminModel;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using kajiApp_blazor.Domain.Entity;
using kajiApp_blazor.Infra.DTO.EatModel;

namespace kajiApp_blazor.ViewModel.AdminDBC
{
    public class PaymentDetailOperation
    {
        private readonly kajiappDBContext _context;
        public PaymentDetailOperation(kajiappDBContext context)
        {
                _context = context;
        }
        public async Task<List<PaymentDetail>> GetPaymentDetailAsync()
        {

            var paydetail = await _context.LifeDetailSummaries
                .Join(
                    _context.Payments,
                    payment => payment.Yyyymm,  // 支払いテーブルの結合キー
                    LifeDetailSummarie => LifeDetailSummarie.Yyyymm, // 名前リストの結合キー
                    (LifeDetailSummarie, payment) => new  // 結合結果（Entity のまま）
                    {
                        LifeDetailSummarie.Yyyymm,
                        LifeDetailSummarie.食費,
                        LifeDetailSummarie.家賃,
                        LifeDetailSummarie.水道代,
                        LifeDetailSummarie.電気代,
                        LifeDetailSummarie.ガス代,
                        LifeDetailSummarie.生活費食費,
                        LifeDetailSummarie.生活費合計,
                        LifeDetailSummarie.折半計算,
                        LifeDetailSummarie.家事割合適用後折半代金,
                        LifeDetailSummarie.荻田,
                        payment.Pay,
                        payment.NameCode,
                        payment.決裁
                    }
                )
                .Where(p => p.決裁 != "済" ) // 条件フィルタ
                .OrderBy(p => p.Yyyymm)
                .ToListAsync();

            // ② DTO に変換
            var paymentDetail = paydetail.Select(p => new PaymentDetail
            {
                YearMonth = p.Yyyymm,
                Food = p.食費,
                Rent = p.家賃,
                Water = p.水道代,
                Electricity = p.電気代,
                Gas = p.ガス代,
                LifeTotal = p.生活費食費,
                FullTotal = p.生活費合計,
                SharedCost = p.折半計算,
                AdjustedSharedCost = p.家事割合適用後折半代金,
                Percentage = p.荻田,
                Payment = p.Pay?.ToString() ?? "0",   // `null` の場合は "0" をセット
                Payer = p.NameCode?.ToString() ?? "N/A", // `null` の場合は "N/A" をセット
                Status = p.決裁
            }).ToList();

            return paymentDetail; // DTO を返す

        }
    }
}
