using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Threading.Tasks;
using MySqlConnector;
using Npgsql;
using Npgsql.Schema;

namespace EeveeTools.Database {
    public class DatabaseContext {
        private readonly string _username, _password, _location, _database;
        public DatabaseContext(string username, string password, string location, string database) {
            this._username    = username;
            this._password    = password;
            this._location    = location;
            this._database    = database;
        }
        public string GetMySqlConnectionString() => $"server={this._location};userid={this._username};password={this._password};database={this._database}";
        public string GetNpgsqlConnectionString() => $"Host={this._location};Username={this._username};Password={this._password};Database={this._database}";
    }

    public static partial class DatabaseContextExtensions {
        /// <summary>
        /// Executes a Query asynchronously
        /// </summary>
        /// <param name="context">Database Context</param>
        /// <param name="query">SQL</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>awaitable Task</returns>
        public static async Task<IReadOnlyDictionary<string, object>[]> MySqlQueryAsync(DatabaseContext context, string query, MySqlParameter[] parameters = null) {
            //Place to store Results
            List<IReadOnlyDictionary<string, object>> results = new();
            //Create and Open Connection
            MySqlConnection connection = new(context.GetMySqlConnectionString());
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
        /// <summary>
        /// Executes a Query
        /// </summary>
        /// <param name="context">Database Context</param>
        /// <param name="query">SQL</param>
        /// <param name="parameters">Parameters</param>
        public static IReadOnlyDictionary<string, object>[] MySqlQuery(this DatabaseContext context, string query, MySqlParameter[] parameters = null) {
            //Place to store Results
            List<IReadOnlyDictionary<string, object>> results = new();
            //Create and Open Connection
            MySqlConnection connection = new(context.GetMySqlConnectionString());
            connection.Open();
            //Create command with `query` Command Text
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = query;
            //Add Parameters if present
            if(parameters != null)
                cmd.Parameters.AddRange(parameters);
            //Execute Data Reader
            using MySqlDataReader reader = cmd.ExecuteReader();
            //Read Columns
            ReadOnlyCollection<DbColumn> columns = reader.GetColumnSchema();
            //Read Results
            while (reader.Read()) {
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
            connection.Close();
            //Return Results
            return results.ToArray();
        }
        /// <summary>
        /// Executes a Non Query
        /// </summary>
        /// <param name="context">Database Context</param>
        /// <param name="query">SQL</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>awaitable Task</returns>
        public static void MySqlNonQuery(this DatabaseContext context, string query, MySqlParameter[] parameters = null) {
            //Creates a new Connection
            using MySqlConnection connection = new(context.GetMySqlConnectionString());
            //Open Connection
            connection.Open();
            //Creates a Command and assings the Command Text to desired Query
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = query;
            //Add Parameters if there arent any
            if(parameters != null)
                command.Parameters.AddRange(parameters);
            //Execute Non Query
            command.ExecuteNonQuery();
            //Close Connection
            connection.Close();
        }
        /// <summary>
        /// Executes a Non Query Asynchonously
        /// </summary>
        /// <param name="context">Database Context</param>
        /// <param name="query">SQL</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>awaitable Task</returns>
        public static async Task MySqlNonQueryAsync(this DatabaseContext context, string query, MySqlParameter[] parameters = null) {
            //Creates a new Connection
            await using MySqlConnection connection = new(context.GetMySqlConnectionString());
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
        /// <summary>
        /// Executes a Query asynchronously
        /// </summary>
        /// <param name="context">Database Context</param>
        /// <param name="query">SQL</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>awaitable Task</returns>
        public static async Task<IReadOnlyDictionary<string, object>[]> NpgsqlQueryAsync(this DatabaseContext context, string query, NpgsqlParameter[] parameters = null) {
            //Place to store Results
            List<IReadOnlyDictionary<string, object>> results = new();
            //Create and Open Connection
            NpgsqlConnection connection = new(context.GetNpgsqlConnectionString());
            await connection.OpenAsync();
            //Create command with `query` Command Text
            NpgsqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = query;
            //Add Parameters if present
            if(parameters != null)
                cmd.Parameters.AddRange(parameters);
            //Execute Data Reader
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            //Read Columns
            ReadOnlyCollection<NpgsqlDbColumn> columns = await reader.GetColumnSchemaAsync();
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
                    NpgsqlDbColumn column = columns[i];
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
        /// <summary>
        /// Executes a Query
        /// </summary>
        /// <param name="context">Database Context</param>
        /// <param name="query">SQL</param>
        /// <param name="parameters">Parameters</param>
        public static IReadOnlyDictionary<string, object>[] NpgsqlQuery(this DatabaseContext context, string query, NpgsqlParameter[] parameters = null) {
            //Place to store Results
            List<IReadOnlyDictionary<string, object>> results = new();
            //Create and Open Connection
            NpgsqlConnection connection = new(context.GetNpgsqlConnectionString());
            connection.Open();
            //Create command with `query` Command Text
            NpgsqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = query;
            //Add Parameters if present
            if(parameters != null)
                cmd.Parameters.AddRange(parameters);
            //Execute Data Reader
            using NpgsqlDataReader reader = cmd.ExecuteReader();
            //Read Columns
            ReadOnlyCollection<NpgsqlDbColumn> columns = reader.GetColumnSchema();
            //Read Results
            while (reader.Read()) {
                //Place to store Result
                Dictionary<string, object> result = new();
                //Create Object Array with the size of the amount of columns
                object[] values = new object[columns.Count];
                //Get Values
                reader.GetValues(values);
                //Iterate over each column
                for (int i = 0; i < columns.Count; i++) {
                    //Get Column
                    NpgsqlDbColumn column = columns[i];
                    //Add Result
                    result.Add(column.ColumnName, values[i]);
                }
                //Add Result to Results
                results.Add(result);
            }
            //Close Connection
            connection.Close();
            //Return Results
            return results.ToArray();
        }
        /// <summary>
        /// Executes a Non Query
        /// </summary>
        /// <param name="context">Database Context</param>
        /// <param name="query">SQL</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>awaitable Task</returns>
        public static void NpgsqlNonQuery(this DatabaseContext context, string query, NpgsqlParameter[] parameters = null) {
            //Creates a new Connection
            using NpgsqlConnection connection = new(context.GetNpgsqlConnectionString());
            //Open Connection
            connection.Open();
            //Creates a Command and assings the Command Text to desired Query
            NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = query;
            //Add Parameters if there arent any
            if(parameters != null)
                command.Parameters.AddRange(parameters);
            //Execute Non Query
            command.ExecuteNonQuery();
            //Close Connection
            connection.Close();
        }
        /// <summary>
        /// Executes a Non Query Asynchonously
        /// </summary>
        /// <param name="context">Database Context</param>
        /// <param name="query">SQL</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>awaitable Task</returns>
        public static async Task NpgsqlNonQueryAsync(this DatabaseContext context, string query, NpgsqlParameter[] parameters = null) {
            //Creates a new Connection
            await using NpgsqlConnection connection = new(context.GetNpgsqlConnectionString());
            //Open Connection
            await connection.OpenAsync();
            //Creates a Command and assings the Command Text to desired Query
            NpgsqlCommand command = connection.CreateCommand();
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
