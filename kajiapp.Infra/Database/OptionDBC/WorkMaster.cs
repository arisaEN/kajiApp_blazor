﻿using System.Data.SQLite;
using kajiApp_blazor.Infra.DTO.HomeModel;
using kajiApp_blazor.Infra.DTO.OptionModel;
using kajiApp_blazor.Domain.Entity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using kajiApp_blazor.Infra.DTO.EatModel;


namespace kajiApp_blazor.Database.OptionDBC
{
    public class WorkMaster
    {
        private readonly kajiappDBContext _context;
        public WorkMaster(kajiappDBContext context)
        {
            _context = context;
        }
        //private readonly string _connectionString = "Data Source=database.db";
        //非同期版
        public async Task<List<WorkMasterList>> GetWorkMasterAsync() // ✅ 非同期メソッド
        {
            // ① Entity を取得
            var workLists = await _context.WorkLists
                .OrderByDescending(w => w.WorkId)
                .Select(w => new
                {
                    w.WorkId,
                    w.WorkName,
                    w.WorkNamePoint,
                    w.家事分類区分番号
                })
                .ToListAsync();

            // ② DTO に変換
            var workMasterList = workLists.Select(w => new WorkMasterList
            {
                Id =w.WorkId,
                WorkName = w.WorkName,
                WorkNamePoint = w.WorkNamePoint ?? 0,
                CategoryNumber = w.家事分類区分番号 ?? 0
            }).ToList();

            return workMasterList; // DTO を返す
        }
        /// <summary>
        /// 仕事マスタ登録
        /// </summary>
        /// <returns></returns>
        public async Task InsertWorkMasterAsync(WorkMasterList newWork)
        {
            //Entityモデルをnew 引数を代入する。
            var workList = new Domain.Entity.WorkList
            {
                WorkId = newWork.Id,
                WorkName = newWork.WorkName,
                WorkNamePoint = newWork.WorkNamePoint,
                家事分類区分番号 = newWork.CategoryNumber
            };
            //レコード追加とDB保存
            await _context.WorkLists.AddAsync(workList);
            await _context.SaveChangesAsync();
        }
    }
}
