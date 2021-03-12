using System;
using EeveeTools.Servers.TCP;

namespace EeveeTools.TestApp {
    class Program {
        static void Main(string[] args) {
            TcpServer server = new("127.0.0.1", 13382, typeof(Handler));
            server.Start();
            Console.ReadLine();
        }
    }
}
