namespace EeveeTools.Database {
    public class DatabaseContext {
        private string Username, Password, Location, Database;

        public DatabaseContext(string Username, string Password, string Location, string Database) {
            this.Username = Username;
            this.Password = Password;
            this.Location = Location;
            this.Database = Database;
        }

        public string GetConnectionString() => $"server={this.Location};userid={this.Username};pasword={this.Password};database={this.Database}";
    }
}
