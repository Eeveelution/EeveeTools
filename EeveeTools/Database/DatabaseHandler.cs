using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Threading.Tasks;
using MySqlConnector;

namespace EeveeTools.Database {
    public class DatabaseHandler {
        public static async Task<IReadOnlyDictionary<string, object>> QueryAsync(string Query, MySqlParameter[] Parameters, DatabaseContext Context) {
            Dictionary<string, object> results = new();

            await using MySqlConnection connection = new(Context.GetConnectionString());

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = Query;

            if(Parameters != null)
                command.Parameters.AddRange(Parameters);

            await using MySqlDataReader reader = await command.ExecuteReaderAsync();
            ReadOnlyCollection<DbColumn> columns = await reader.GetColumnSchemaAsync();

            while (await reader.NextResultAsync()) {
                object[] values = new object[columns.Count];
                reader.GetValues(values);

                for (int i = 0; i != columns.Count; i++) {
                    DbColumn column = columns[i];
                    results.Add(column.ColumnName, values[i]);
                }
            }

            return results;
        }
    }
}
