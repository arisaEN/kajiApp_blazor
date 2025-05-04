using System;
using Microsoft.Data.Sqlite;

namespace kajiApp_blazor.Database
{
    /// <summary>
    /// DBの初期化処理 月毎にその月のレコードがなければ生成
    /// </summary>
    public class InitDBC
    {
        private const string DatabaseFile = "database.db";

        public static void InitializeDatabase()
        {
            var con = new SqliteConnection($"Data Source={DatabaseFile};");
            {
                con.Open();
                InsertCurrentMonthPaymentDetail(con);
                InsertCurrentMonthEatRecord(con);
                InsertLifeDetailIfNotExists(con);
            }
        }

        private static void InsertCurrentMonthPaymentDetail(SqliteConnection con)
        {
            string currentYear = DateTime.Now.Year.ToString("0000");
            string currentMonth = DateTime.Now.Month.ToString("00");
            string yyyymm = $"{currentYear}{currentMonth}";

            using (var cmd = new SqliteCommand("SELECT COUNT(*) FROM payment WHERE yyyymm = @yyyymm", con))
            {
                cmd.Parameters.AddWithValue("@yyyymm", yyyymm);
                long count = (long)cmd.ExecuteScalar();

                if (count == 0)
                {
                    using (var insertCmd = new SqliteCommand(
                        "INSERT INTO payment (year, month, pay, name_code) VALUES (@year, @month, 0, @name_code)", con))
                    {
                        insertCmd.Parameters.AddWithValue("@year", currentYear);
                        insertCmd.Parameters.AddWithValue("@month", currentMonth);
                        insertCmd.Parameters.AddWithValue("@name_code", 1); // 適切な値に変更してください
                        insertCmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private static void InsertCurrentMonthEatRecord(SqliteConnection con)
        {
            string currentYear = DateTime.Now.Year.ToString("0000");
            string currentMonth = DateTime.Now.Month.ToString("00");

            using (var cmd = new SqliteCommand("SELECT 1 FROM eat WHERE year = @year AND month = @month", con))
            {
                cmd.Parameters.AddWithValue("@year", currentYear);
                cmd.Parameters.AddWithValue("@month", currentMonth);
                var recordExists = cmd.ExecuteScalar();

                if (recordExists == null)
                {
                    using (var insertCmd = new SqliteCommand(
                        "INSERT INTO eat (year, month, amount, description) VALUES (@year, @month, @amount, @description)", con))
                    {
                        insertCmd.Parameters.AddWithValue("@year", currentYear);
                        insertCmd.Parameters.AddWithValue("@month", currentMonth);
                        insertCmd.Parameters.AddWithValue("@amount", 0.0);
                        insertCmd.Parameters.AddWithValue("@description", "初期レコード");
                        insertCmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private static void InsertLifeDetailIfNotExists(SqliteConnection con)
        {
            string currentYear = DateTime.Now.Year.ToString("0000");
            string currentMonth = DateTime.Now.Month.ToString("00");

            using (var cmd = new SqliteCommand("SELECT COUNT(*) FROM life_detail WHERE year = @year AND month = @month", con))
            {
                cmd.Parameters.AddWithValue("@year", currentYear);
                cmd.Parameters.AddWithValue("@month", currentMonth);
                long count = (long)cmd.ExecuteScalar();

                if (count == 0)
                {
                    using (var insertCmd = new SqliteCommand(
                        "INSERT INTO life_detail (year, month, rent, water, electricity, gas, input_time) VALUES (@year, @month, 0, 0, 0, 0, CURRENT_TIMESTAMP)", con))
                    {
                        insertCmd.Parameters.AddWithValue("@year", currentYear);
                        insertCmd.Parameters.AddWithValue("@month", currentMonth);
                        insertCmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
