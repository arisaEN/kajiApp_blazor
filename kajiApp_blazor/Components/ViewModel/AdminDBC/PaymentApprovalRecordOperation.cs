using kajiApp_blazor.Components.DataModels.AdminModel;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kajiApp_blazor.Components.ViewModel.AdminDBC
{
    public class PaymentApprovalRecordOperation
    {
        private readonly string _connectionString = "Data Source=database.db";

        public async Task<List<PaymentApprovalRecord>> GetPaymentApprovalRecordAsync()
        {
            var paymentApprovalRecord = new List<PaymentApprovalRecord>();

            try
            {
                using var connection = new SqliteConnection(_connectionString);
                await connection.OpenAsync(); // ✅ 非同期でDB接続を開く

                var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT payment.yyyymm, nameList.name, payment.pay, payment.""決裁"", id
                    FROM payment
                    JOIN nameList ON payment.name_code = nameList.name_id
                    WHERE payment.""決裁"" <> '済' 
                        OR payment.""決裁"" IS NULL
                    ORDER BY payment.yyyymm ASC;";

                using var reader = await command.ExecuteReaderAsync(); // ✅ 非同期実行
                while (await reader.ReadAsync()) // ✅ 非同期で1行ずつ読み取る
                {
                    paymentApprovalRecord.Add(new PaymentApprovalRecord
                    {
                        YearMonth = reader.GetString(0),
                        Payer = reader.GetString(1),
                        Payment = !reader.IsDBNull(2) ? reader.GetInt32(2) : 0, // ✅ NULLの場合は0を設定
                        Status = !reader.IsDBNull(3) ? reader.GetString(3) : string.Empty, // ✅ NULLの場合は空文字を設定
                        Id = reader.GetInt32(4) // ✅ Idを取得
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"エラー: {ex.Message}");
                Console.WriteLine($"スタックトレース: {ex.StackTrace}");
            }

            return paymentApprovalRecord; // ✅ エラー時も空リストを返す
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
                using var connection = new SqliteConnection(_connectionString);
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "UPDATE payment " +
                                      "SET pay = @Payment, 決裁 = @Status " +
                                      "WHERE yyyymm = @YearMonth";
                command.Parameters.AddWithValue("@Payment", Payment);
                command.Parameters.AddWithValue("@YearMonth", YearMonth);
                command.Parameters.AddWithValue("@Status", Status);

                int rowsAffected = await command.ExecuteNonQueryAsync(); // 更新された行数を取得
                return rowsAffected > 0; // 1 以上なら成功
            }
            catch (Exception ex)
            {
                Console.WriteLine($"データ更新中にエラーが発生しました: {ex.Message}");
                return false; // 失敗
            }
        }

    }
}