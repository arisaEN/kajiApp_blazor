using kajiApp_blazor.Components.DTO.AdminModel;
using Microsoft.Data.Sqlite;

namespace kajiApp_blazor.Components.ViewModel.AdminDBC
{
    public class PaymentDetailOperation
    {
        private readonly string _connectionString = "Data Source=database.db";
        public async Task<List<PaymentDetail>> GetPaymentDetailAsync()
        {
            //await Task.Delay(1000);
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync(); // ✅ OpenAsync() を使う

            var command = connection.CreateCommand();
            command.CommandText = "SELECT *  "+
                                                    "FROM life_detail_summary " +
                                                    "JOIN payment ON life_detail_summary.yyyymm = payment.yyyymm " +
                                                    "WHERE payment.決裁<> '済' " +
                                                    "OR payment.決裁 is null " +
                                                    "ORDER BY yyyymm asc; ";

            var paymentDetail = new List<PaymentDetail>();
            //sqliteのExecuteReaderAsyncは1明細ずつ取得する仕組みらしい
            using var reader = await command.ExecuteReaderAsync(); // ✅ 非同期実行
            //データをある分だけ1明細ずつ取得 
            while (await reader.ReadAsync()) // ✅ 非同期読み取り
            {
                paymentDetail.Add(new PaymentDetail
                {
                    YearMonth = reader.GetString(0),
                    Food = reader.GetString(1),
                    Rent = reader.GetString(2),
                    Water = reader.GetString(3),
                    Electricity = reader.GetString(4),
                    Gas = reader.GetString(5),
                    LifeTotal = reader.GetString(6),
                    FullTotal = reader.GetString(7),
                    SharedCost = reader.GetString(8),
                    AdjustedSharedCost = reader.GetString(9),
                    Percentage = reader.GetString(10),
                    Payment = reader.GetString(11),
                    Payer = reader.GetString(12),
                    Status = reader.GetString(13)
                });
            }
            return paymentDetail; // ✅ 戻り値を Task<List<Work>> にする

        }
    }
}
