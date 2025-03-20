using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Text.Json;
using kajiApp_blazor.Infra.DTO.AdminModel;
using kajiApp_blazor.Infra.DTO.EatModel;
using kajiApp_blazor.Domain.Entity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace kajiApp_blazor.ViewModel.AdminDBC

{
    public class EatRecordShow
    {
        private readonly kajiappDBContext _context;
        private JsonDocument jsonString;

        public EatRecordShow(kajiappDBContext context)
        {
            _context = context;
        }
        //private readonly string _connectionString = "Data Source=database.db";
        //非同期版
        public async Task<List<EatRecord>> GetEatAsync() // ✅ 非同期メソッド
        {
            // ① Entity を取得
            var eats = await _context.Eats
                .OrderByDescending(e => e.Yyyymm)
                .Select(e => new
                {
                    e.Year,
                    e.Month,
                    e.Amount
                })
                .ToListAsync();

            // ② DTO に変換
            var eatrecord = eats.Select(e => new EatRecord
            {
                Year = e.Year,
                Month = e.Month ,
                Amount = e.Amount ?? 0
            }).ToList();
            //Console.WriteLine(eatrecord.GetType());  // 確認用
            //eatrecord = JsonSerializer.Deserialize<List<EatRecord>>(jsonString);
            return eatrecord; // DTO を返す
        }
    }
}