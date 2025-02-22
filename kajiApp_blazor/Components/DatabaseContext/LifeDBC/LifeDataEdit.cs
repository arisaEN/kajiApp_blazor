using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using kajiApp_blazor.Components.Models.LifeModel;
using Microsoft.Data.Sqlite;
using kajiApp_blazor.Components.DatabaseContext.LifeDBC;

namespace kajiApp_blazor.Components.DatabaseContext.LifeDBC
{
    public class LifeDataEdit
    {
        private readonly string _connectionString = "Data Source=database.db";
        /// <summary>
        /// life明細更新
        /// </summary>
        /// <param name="record"></param>
        public async Task UpdateEatDetailAsync(Models.LifeModel.LifeRecord record)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();
            var command = connection.CreateCommand();
            command.CommandText = "UPDATE life_detail " +
                                                      "SET rent = @rent, " +  // ← カンマ追加
                                                      "    water = @water, " + // ← カンマ追加
                                                      "    electricity = @electricity, " + // ← カンマ追加
                                                      "    gas = @gas " + // ← 最後は不要
                                                      "WHERE id = @id";

            command.Parameters.AddWithValue("@id", record.id);
            command.Parameters.AddWithValue("@rent", record.rent);
            command.Parameters.AddWithValue("@water", record.water);
            command.Parameters.AddWithValue("@electricity", record.electricity);
            command.Parameters.AddWithValue("@gas", record.gas);

            await command.ExecuteNonQueryAsync();
        }
    }
    
}
