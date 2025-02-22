using System.Text.RegularExpressions;
using System.Xml.Linq;
using kajiApp_blazor.Components.Models.HomeModel;
using Microsoft.Data.Sqlite;

namespace kajiApp_blazor.Components.DatabaseContext.HomeDBC
{
    public class PointSummaryShow
    {
        private readonly string _connectionString = "Data Source=database.db";
        //非同期版
        public async Task<List<PointSummary>> GetPointSumListAsync() // ✅ 非同期メソッド
        {
            await Task.Delay(1000);
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync(); // ✅ OpenAsync() を使う

            var command = connection.CreateCommand();
            command.CommandText = "    SELECT works.name, " +
                                                           " SUM(workList.workNamePoint * (works.percent * 0.01)) AS total_points, " +
                                                           "CAST(SUM(workList.workNamePoint * (works.percent * 0.01)) / " +
                                                           "     (SELECT SUM(workList.workNamePoint * (works.percent * 0.01)) " +
                                                           "         FROM works " +
                                                           "         JOIN workList ON works.work_id = workList.work_id " +
                                                           "         WHERE strftime('%Y-%m', works.day) = strftime('%Y-%m', 'now')) *100 AS INTEGER) AS percentage " +
                                                           "         FROM works " +
                                                           "         JOIN workList ON works.work_id = workList.work_id " +
                                                           "         WHERE strftime('%Y-%m', works.day) = strftime('%Y-%m', 'now') " +
                                                           "         GROUP BY works.name " +
                                                           "         ORDER BY total_points DESC;  ";

            var pointsummary = new List<PointSummary>();
            //sqliteのExecuteReaderAsyncは1明細ずつ取得する仕組みらしい
            using var reader = await command.ExecuteReaderAsync(); // ✅ 非同期実行
            //データをある分だけ1明細ずつ取得 
            while (await reader.ReadAsync()) // ✅ 非同期読み取り
            {
                pointsummary.Add(new PointSummary
                {
                    Name = reader.GetString(0),
                    TotalPoints = reader.GetInt32(1),
                    Percentage = reader.GetDouble(2)
                });
            }
            return pointsummary; // ✅ 戻り値を Task<List<Work>> にする
        }

    }
}