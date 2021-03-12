using System;
using System.Net.Sockets;

namespace EeveeTools.Servers.TCP {
    public abstract class TcpClientHandler {
        protected TcpClient     _client;
        protected NetworkStream _stream;

        public void HandleNew(TcpClient Client) {
            this._client = Client;
            this._stream = this._client.GetStream();

            this.HandleConnection();
        }

        protected abstract void HandleData(byte[] Data);

        public void HandleConnection() {
            byte[] readBuffer = new byte[4096];
            while (this._client.Connected) {
                if (this._client.Available != 0 && this._stream.DataAvailable && this._stream.CanRead) {
                    int bytesRecieved = this._stream.Read(readBuffer, 0, 4096);
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
