using System.Data.SQLite;
using kajiApp_blazor.Infra.DTO.OptionModel;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using kajiApp_blazor.Domain.Entity;
using kajiApp_blazor.Infra.DTO.EatModel;


namespace kajiApp_blazor.Database.OptionDBC
{
    public class NameMaster
    {
        //private readonly string _connectionString = "Data Source=database.db";
        private readonly kajiappDBContext _context;
        public NameMaster(kajiappDBContext context)
        {
            _context = context;
        }
        //非同期版
        public async Task<List<NameMasterList>> GetNameMasterAsync() // ✅ 非同期メソッド
        {
            // ① Entity を取得
            var nameLists = await _context.NameLists
                .OrderByDescending(n => n.NameId)
                .Select(n => new
                {
                    n.NameId,
                    n.Name
                })
                .ToListAsync();

            // ② DTO に変換
            var nameMasterList = nameLists.Select(n => new NameMasterList
            {
                Id = n.NameId,
                Name = n.Name
            }).ToList();

            return nameMasterList; // DTO を返す
        }

        public async Task InsertNameMasterAsync(NameMasterList newName)
        {
            //Entityモデルをnew 引数を代入する。
            var nameList = new NameList
            {
                NameId = newName.Id,
                Name = newName.Name
            };
            //レコード追加とDB保存
            await _context.NameLists.AddAsync(nameList);
            await _context.SaveChangesAsync();

        }
    }
}
