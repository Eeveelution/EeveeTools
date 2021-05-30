using System;
using System.Net.Sockets;
using System.Threading;

namespace EeveeTools.Servers.TCP {
    public abstract class TcpClientHandler {
        protected TcpClient     Client;
        protected NetworkStream Stream;

        private object _sendLock = new();

        public void HandleNew(TcpClient client) {
            this.Client = client;
            this.Stream = this.Client.GetStream();

            this.HandleConnection();
        }

        protected abstract void HandleData(byte[] data);
        protected abstract void HandleDisconnect();

        public void HandleConnection() {
            try {
                //Create Buffer to hold Data In
                byte[] readBuffer = new byte[4096];
                //Read While Connected
                while (this.Client.Connected) {
                    if (this.Client.Available != 0 && this.Stream.DataAvailable && this.Stream.CanRead) {
                        int bytesRecieved = this.Stream.Read(readBuffer, 0, 4096);
                        //Cut Buffer
                        byte[] destinationBuffer = new byte[bytesRecieved];
                        Buffer.BlockCopy(readBuffer, 0, destinationBuffer, 0, bytesRecieved);
                        //Invoke Method
                        this.HandleData(destinationBuffer);
                    }
                    Thread.Sleep(25);
                }
                this.HandleDisconnect();
            }
            catch {
                /**/
            }
        }

        public void SendData(byte[] data) {
            try {
                lock (this._sendLock)
                    this.Stream.Write(data);
            }
            catch {
                this.Stream.Dispose();
                this.Client.Dispose();
            }
        }
    }
}
