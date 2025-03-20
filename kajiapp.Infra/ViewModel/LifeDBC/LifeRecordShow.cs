using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Reflection.PortableExecutable;
using kajiApp_blazor.Infra.DTO.EatModel;
using kajiApp_blazor.Infra.DTO.LifeModel;
using kajiApp_blazor.Domain.Entity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;


namespace kajiApp_blazor.ViewModel.LifeDBC
{
    public class LifeRecordShow
    {
        //private readonly string _connectionString = "Data Source=database.db";
        private readonly kajiappDBContext _context;
        public LifeRecordShow(kajiappDBContext context)
        {
            _context = context;
        }
        //非同期版
        public async Task<List<LifeRecord>> GetLifeAsync() // ✅ 非同期メソッド
        {
            // ① Entity を取得
            var lifeDetails = await _context.LifeDetails
                .OrderByDescending(e => e.Yyyymm)
                .Select(l => new
                {
                    l.Id,
                    l.Year,
                    l.Month,
                    l.Rent,
                    l.Water,
                    l.Electricity,
                    l.Gas
                })
                .ToListAsync();

            // ② DTO に変換
            var lifeRecord = lifeDetails.Select(l => new LifeRecord
            {
                Id = l.Id,
                Year = l.Year,
                Month = l.Month,
                Rent = l.Rent ?? 0, // null の場合は 0 に設定
                Water = l.Water ?? 0, // null の場合は 0 に設定
                Electricity = l.Electricity ?? 0, // null の場合は 0 に設定
                Gas = l.Gas ?? 0 // null の場合は 0 に設定
            }).ToList();

            return lifeRecord; // DTO を返す

        }
    }
}
