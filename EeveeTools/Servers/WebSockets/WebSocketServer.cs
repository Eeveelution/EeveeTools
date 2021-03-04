using System;
using Fleck;

namespace EeveeTools.Servers.WebSockets {
    public abstract class WebSocketServer {
        protected readonly Fleck.WebSocketServer _webSocketServer;

        public WebSocketServer(string Location) {
            //Create Server
            this._webSocketServer = new Fleck.WebSocketServer(Location);
            //Start
            this._webSocketServer.Start(this.InitlializeSocket);
        }

        private void InitlializeSocket(IWebSocketConnection Connection) {
            Connection.OnOpen    += this.OnConnectionOpen;
            Connection.OnBinary  += this.OnBinaryData;
            Connection.OnClose   += this.OnConnectionClose;
            Connection.OnError   += this.OnConnectionError;
            Connection.OnMessage += this.OnMessage;
            Connection.OnPing    += this.OnConnectionPing;
            Connection.OnPong    += this.OnConnectionPong;
        }

        #region Abstracts

        protected abstract void OnConnectionOpen();
        protected abstract void OnBinaryData(byte[] Data);
        protected abstract void OnConnectionClose();
        protected abstract void OnConnectionError(Exception Error);
        protected abstract void OnMessage(string Message);
        protected virtual void OnConnectionPing(byte[] Data) {}
        protected virtual void OnConnectionPong(byte[] Data) {}


        #endregion

    }
}
