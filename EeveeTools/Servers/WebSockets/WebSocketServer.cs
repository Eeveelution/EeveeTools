using System;
using Fleck;

namespace EeveeTools.Servers.WebSockets {
    /// <summary>
    /// WebSocket Server which is easy to Set Up and get Running
    /// </summary>
    public abstract class WebSocketServer {
        protected readonly Fleck.WebSocketServer Server;

        public WebSocketServer(string location, Type clientHandlerType) {
            //Create Server
            this.Server = new Fleck.WebSocketServer(location);
            //Start
            this.Server.Start(this.InitlializeSocket);
        }

        private void InitlializeSocket(IWebSocketConnection connection) {
            connection.OnOpen    += this.OnConnectionOpen;
            connection.OnBinary  += this.OnBinaryData;
            connection.OnClose   += this.OnConnectionClose;
            connection.OnError   += this.OnConnectionError;
            connection.OnMessage += this.OnMessage;
            connection.OnPing    += this.OnConnectionPing;
            connection.OnPong    += this.OnConnectionPong;
        }

        #region Abstracts

        protected abstract void OnConnectionOpen();
        protected abstract void OnBinaryData(byte[] data);
        protected abstract void OnConnectionClose();
        protected abstract void OnConnectionError(Exception error);
        protected abstract void OnMessage(string message);
        protected virtual void OnConnectionPing(byte[] data) {}
        protected virtual void OnConnectionPong(byte[] data) {}


        #endregion

    }
}
