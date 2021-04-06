using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Threading.Tasks;
using MySqlConnector;

namespace EeveeTools.Database {
    public class DatabaseHandler {
        public static async Task<IReadOnlyDictionary<string, object>[]> QueryAsync(DatabaseContext context, string query, MySqlParameter[] parameters = null) {
            //Place to store Results
            List<IReadOnlyDictionary<string, object>> results = new();
            //Create and Open Connection
            MySqlConnection connection = new(context.GetConnectionString());
            await connection.OpenAsync();
            //Create command with `query` Command Text
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = query;
            //Add Parameters if present
            if(parameters != null)
                cmd.Parameters.AddRange(parameters);
            //Execute Data Reader
            await using MySqlDataReader reader = await cmd.ExecuteReaderAsync();
            //Read Columns
            ReadOnlyCollection<DbColumn> columns = await reader.GetColumnSchemaAsync();
            //Read Results
            while (await reader.ReadAsync()) {
                //Place to store Result
                Dictionary<string, object> result = new();
                //Create Object Array with the size of the amount of columns
                object[] values = new object[columns.Count];
                //Get Values
                reader.GetValues(values);
                //Iterate over each column
                for (int i = 0; i < columns.Count; i++) {
                    //Get Column
                    DbColumn column = columns[i];
                    //Add Result
                    result.Add(column.ColumnName, values[i]);
                }
                //Add Result to Results
                results.Add(result);
            }
            //Close Connection
            await connection.CloseAsync();
            //Return Results
            return results.ToArray();
        }
        public static async Task Insert(DatabaseContext context, string query, MySqlParameter[] parameters = null) {
            //Creates a new Connection
            await using MySqlConnection connection = new(context.GetConnectionString());
            //Open Connection
            await connection.OpenAsync();
            //Creates a Command and assings the Command Text to desired Query
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = query;
            //Add Parameters if there arent any
            if(parameters != null)
                command.Parameters.AddRange(parameters);
            //Execute Non Query
            await command.ExecuteNonQueryAsync();
            //Close Connection
            await connection.CloseAsync();
        }
    }
}
