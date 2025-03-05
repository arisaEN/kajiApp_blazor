using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using kajiApp_blazor.Components.DataModels.LifeModel;
using Microsoft.Data.Sqlite;
using kajiApp_blazor.Components.DBx.LifeDBC;

namespace kajiApp_blazor.Components.DBx.LifeDBC
{
    public class LifeDataEdit
    {
        private readonly string _connectionString = "Data Source=database.db";
        /// <summary>
        /// life明細更新
        /// </summary>
        /// <param name="record"></param>
        public async Task UpdateEatDetailAsync(DataModels.LifeModel.LifeRecord record)
        {
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText = @"
            UPDATE life_detail 
            SET rent = @rent, 
                water = @water, 
                electricity = @electricity, 
                gas = @gas 
            WHERE id = @id";

                command.Parameters.AddWithValue("@id", record.Id);
                command.Parameters.AddWithValue("@rent", record.Rent);
                command.Parameters.AddWithValue("@water", record.Water);
                command.Parameters.AddWithValue("@electricity", record.Electricity);
                command.Parameters.AddWithValue("@gas", record.Gas);

                int affectedRows = await command.ExecuteNonQueryAsync();
                if (affectedRows == 0)
                {
                    Debug.WriteLine($"更新失敗: ID {record.Id} のレコードが見つかりません");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"データ更新エラー: {ex.Message}");
            }
        }
    }
    
}
