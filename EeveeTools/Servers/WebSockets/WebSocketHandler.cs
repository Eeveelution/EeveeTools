using System;
using Fleck;

namespace EeveeTools.Servers.WebSockets {
    public abstract class WebSocketHandler {
        protected IWebSocketConnection Connection;
        public void HandleNew(IWebSocketConnection connection) {
            this.Connection = connection;

            this.Connection.OnOpen    += this.OnConnectionOpen;
            this.Connection.OnBinary  += this.OnBinaryData;
            this.Connection.OnClose   += this.OnConnectionClose;
            this.Connection.OnError   += this.OnConnectionError;
            this.Connection.OnMessage += this.OnMessage;
            this.Connection.OnPing    += this.OnConnectionPing;
            this.Connection.OnPong    += this.OnConnectionPong;
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
