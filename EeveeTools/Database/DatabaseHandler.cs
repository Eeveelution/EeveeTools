using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Threading.Tasks;
using MySqlConnector;

namespace EeveeTools.Database {
    public class DatabaseHandler {
        public static async Task<IReadOnlyDictionary<string, object>> QueryAsync(DatabaseContext Context, string Query, MySqlParameter[] Parameters = null) {
            //This is where results will be stored
            Dictionary<string, object> results = new();
            //Creates a new Connection
            await using MySqlConnection connection = new(Context.GetConnectionString());
            //Creates a Command and assings the Command Text to desired Query
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = Query;
            //Add Parameters if there arent any
            if(Parameters != null)
                command.Parameters.AddRange(Parameters);
            //Create Reader and get Schema
            await using MySqlDataReader reader = await command.ExecuteReaderAsync();
            ReadOnlyCollection<DbColumn> columns = await reader.GetColumnSchemaAsync();
            //Read Results
            while (await reader.NextResultAsync()) {
                //Get Values
                object[] values = new object[columns.Count];
                reader.GetValues(values);
                //For all Values, add results
                for (int i = 0; i != columns.Count; i++) {
                    DbColumn column = columns[i];
                    results.Add(column.ColumnName, values[i]);
                }
            }
            //Return Results
            return results;
        }
    }
}
