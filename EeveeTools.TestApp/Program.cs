using System;
using EeveeTools.Extensions;
using EeveeTools.Servers.HTTP;
using EeveeTools.Servers.TCP;

namespace EeveeTools.TestApp {
    class Program {
        static void Main(string[] args) {
            AsyncHttpServer server = new("http://127.0.0.1:80/",
            async (S, Context) => {
                await Context.Response.WriteString("Hello!");
            });
            server.Start();

            Console.ReadLine();
        }
    }
}
