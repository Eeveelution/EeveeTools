using System;
using System.Text;
using EeveeTools.Servers.TCP;

namespace EeveeTools.TestApp {
    public class Handler : TcpClientHandler {

        protected override void HandleData(byte[] Data) {
            Console.WriteLine(Encoding.UTF8.GetString(Data));
        }
    }
}
