using System;
using System.Net.Sockets;

namespace EeveeTools.Servers.TCP {
    public abstract class TcpClientHandler {
        protected TcpClient     Client;
        protected NetworkStream Stream;

        public void HandleNew(TcpClient client) {
            this.Client = client;
            this.Stream = this.Client.GetStream();

            this.HandleConnection();
        }

        protected abstract void HandleData(byte[] data);

        public void HandleConnection() {
            byte[] readBuffer = new byte[4096];
            while (this.Client.Connected) {
                if (this.Client.Available != 0 && this.Stream.DataAvailable && this.Stream.CanRead) {
                    int bytesRecieved = this.Stream.Read(readBuffer, 0, 4096);
                    //Cut Buffer
                    byte[] destinationBuffer = new byte[bytesRecieved];
                    Buffer.BlockCopy(readBuffer, 0, destinationBuffer, 0, bytesRecieved);
                    //Invoke Method
                    this.HandleData(destinationBuffer);
                }
            }
        }

    }
}
