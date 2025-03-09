using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using kajiApp_blazor.Components.DTO.LifeModel;
using Microsoft.Data.Sqlite;
using kajiApp_blazor.Components.ViewModel.LifeDBC;
using kajiApp_blazor.Components.Entity;
using Microsoft.EntityFrameworkCore;

namespace kajiApp_blazor.Components.ViewModel.LifeDBC
{
    public class LifeDataEdit
    {
        //private readonly string _connectionString = "Data Source=database.db";
        private readonly kajiappDBContext _context;
        public LifeDataEdit(kajiappDBContext context)
        {
            _context = context;
        }
        /// <summary>
        /// life明細更新
        /// </summary>
        /// <param name="record"></param>
        public async Task UpdateEatDetailAsync(DTO.LifeModel.LifeRecord record)
        {

            //引数idのレコードを取得 レコードは主キーなので1件しか存在しない想定
            var lifeDetails = await _context.LifeDetails
                                       .FirstOrDefaultAsync(l => l.Id == record.Id);
            if (lifeDetails == null)
            {
                // 該当レコードなし
            }
            lifeDetails.Rent = record.Rent;
            lifeDetails.Water = record.Water;
            lifeDetails.Electricity = record.Electricity;
            lifeDetails.Gas = record.Gas;

            await _context.SaveChangesAsync(); // 更新をデータベースに適用
        }
    }
    
}
