using System.Data.SQLite;
using kajiApp_blazor.Components.DTO.HomeModel;
using kajiApp_blazor.Components.DTO.OptionModel;
using kajiApp_blazor.Components.Entity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using kajiApp_blazor.Components.DTO.EatModel;


namespace kajiApp_blazor.Components.ViewModel.OptionDBC
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





            ////await Task.Delay(1000);
            //using var connection = new SqliteConnection(_connectionString);
            //await connection.OpenAsync(); // ✅ OpenAsync() を使う

            //var command = connection.CreateCommand();
            //command.CommandText = "SELECT * " +
            //                                        "FROM workList " +
            //                                        "ORDER BY work_id desc";

            //var workmaster = new List<WorkMasterList>();
            ////sqliteのExecuteReaderAsyncは1明細ずつ取得する仕組みらしい
            //using var reader = await command.ExecuteReaderAsync(); // ✅ 非同期実行
            ////データをある分だけ1明細ずつ取得 
            //while (await reader.ReadAsync()) // ✅ 非同期読み取り
            //{
            //    workmaster.Add(new WorkMasterList
            //    {
            //        Id = reader.GetInt32(0),
            //        WorkName = reader.GetString(1),
            //        WorkNamePoint = reader.GetInt32(2),
            //        CategoryNumber = reader.GetInt32(3),
            //    });
            //}
            //return workmaster; // ✅ 戻り値を Task<List<Work>> にする
        }
        /// <summary>
        /// 仕事マスタ登録
        /// </summary>
        /// <returns></returns>
        public async Task InsertWorkMasterAsync(WorkMasterList newWork)
        {
            //Entityモデルをnew 引数を代入する。
            var workList = new Entity.WorkList
            {
                WorkId = newWork.Id,
                WorkName = newWork.WorkName,
                WorkNamePoint = newWork.WorkNamePoint,
                家事分類区分番号 = newWork.CategoryNumber
            };
            //レコード追加とDB保存
            await _context.WorkLists.AddAsync(workList);
            await _context.SaveChangesAsync();




            //    try
            //    {
            //        using var connection = new SqliteConnection(_connectionString);
            //        await connection.OpenAsync();
            //        var command = connection.CreateCommand();
            //        command.CommandText = "INSERT INTO workList (workName, workNamePoint, 家事分類区分番号) " +
            //                                                "VALUES (@WorkName,@WorkNamePoint, @CategoryNumber) ";

            //        command.Parameters.AddWithValue("@WorkName", newWork.WorkName);
            //        command.Parameters.AddWithValue("@WorkNamePoint", newWork.WorkNamePoint);
            //        command.Parameters.AddWithValue("@CategoryNumber", newWork.CategoryNumber);


            //        await command.ExecuteNonQueryAsync();

            //        Console.WriteLine("データを登録しました");
            //    }
            //    catch (SQLiteException ex)
            //    {
            //        Console.WriteLine($"SQLite エラー: {ex.Message}");
            //        Console.WriteLine($"スタックトレース: {ex.StackTrace}");
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine($"予期しないエラー: {ex.Message}");
            //        Console.WriteLine($"スタックトレース: {ex.StackTrace}");
            //    }
            //}
        }

    }
}
