namespace EeveeTools.Database {
    public class DatabaseContext {
        private readonly string _username, _password, _location, _database;
        public DatabaseContext(string username, string password, string location, string database) {
            this._username = username;
            this._password = password;
            this._location = location;
            this._database = database;
        }
        public string GetConnectionString() => $"server={this._location};userid={this._username};pasword={this._password};database={this._database}";
    }
}
