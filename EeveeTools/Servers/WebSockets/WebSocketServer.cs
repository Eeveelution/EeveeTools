using System;
using Fleck;

namespace EeveeTools.Servers.WebSockets {
    /// <summary>
    /// WebSocket Server which is easy to Set Up and get Running
    /// </summary>
    public abstract class WebSocketServer {
        /// <summary>
        /// Stores the WebSocket server which is listening for Connections
        /// </summary>
        protected readonly Fleck.WebSocketServer Server;
        /// <summary>
        /// Stores the Client Handler Type which is the Object that will be created every new Connection
        /// </summary>
        private readonly Type _clientHandlerType;
        /// <summary>
        /// Creates a WebSocket Server given location and Client Handler Type
        /// </summary>
        /// <param name="location">Location where to run the Server</param>
        /// <param name="clientHandlerType">Client Handler Type which is the Object that will be created every new Connection</param>
        public WebSocketServer(string location, Type clientHandlerType) {
            //Create Server
            this.Server = new Fleck.WebSocketServer(location);
            //Store Type
            this._clientHandlerType = clientHandlerType;
            //Start
            this.Server.Start(this.InitlializeSocket);
        }
        /// <summary>
        /// Initializes a new <see cref="_clientHandlerType"/> which will be handling the connectioon
        /// </summary>
        /// <param name="connection"></param>
        private void InitlializeSocket(IWebSocketConnection connection) {
            dynamic clientHandler = Activator.CreateInstance(this._clientHandlerType);
            clientHandler?.HandleNew(connection);
        }
    }
}
