using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EeveeTools.Database;
using EeveeTools.Extensions;
using EeveeTools.Servers.HTTP;
using EeveeTools.Servers.TCP;
using MySqlConnector;

namespace EeveeTools.TestApp {
    class Program {
        static async Task Main(string[] args) {
            DatabaseContext context = new("root", "", "127.0.0.1", "flanderemod");

            await DatabaseHandler.Insert(context, "INSERT INTO staff VALUES (@userid, @username, @status)", new []{ new MySqlParameter("@userid", 24), new MySqlParameter("@username", "Eevee"), new MySqlParameter("@status", "pog")});
        }
    }
}
