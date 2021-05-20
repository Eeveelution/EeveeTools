using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace EeveeTools.Servers.TCP {
    public class TcpServer {
        /// <summary>
        /// Provides arguments for the OnClientConnected event
        /// </summary>
        public struct OnClientConnectedEventArgs {
            public TcpClientHandler AcceptedClient;
        }
        /// <summary>
        /// The actual event... not much more
        /// </summary>
        public delegate void OnClientConnected(OnClientConnectedEventArgs args);
        /// <summary>
        /// TCP Listener that is Responsible for Accepting Clients
        /// </summary>
        private readonly TcpListener _tcpListener;
        /// <summary>
        /// Cancellation Token which is Responsible for Stopping the Server
        /// </summary>
        private readonly CancellationTokenSource _cancellationToken;
        /// <summary>
        /// The Type of class to instanciate and
        /// </summary>
        private readonly Type _clientHandlerType;
        /// <summary>
        /// Gets Fired every time a new Connection is accepted
        /// </summary>
        public event OnClientConnected OnClientConnectedEvent;
        /// <summary>
        /// Creates a TCP Server with Handlers at `Location`:`Port`
        /// </summary>
        /// <param name="location">Where to Run</param>
        /// <param name="port">At what Port</param>
        /// <param name="clientHandlerType">Handler for a Connection Open</param>
        public TcpServer(string location, short port, Type clientHandlerType) {
            this._tcpListener       = new TcpListener(IPAddress.Parse(location), port);
            this._cancellationToken = new CancellationTokenSource();
            this._clientHandlerType = clientHandlerType;
        }

        public void Start() {
            //Start Listener
            this._tcpListener.Start();
            //Create Task for Listening
            this.HandleClients();
        }

        private void HandleClients() {
            while (true) {
                //Accept client
                TcpClient acceptedClient = this._tcpListener.AcceptTcpClient();
                //Create Client Handler
                dynamic connectionHandler = Activator.CreateInstance(this._clientHandlerType);
                //Create new Thread and Start
                new Thread(() => {
                    this.OnClientConnectedEvent?.Invoke(new OnClientConnectedEventArgs { AcceptedClient = connectionHandler });
                    connectionHandler?.HandleNew(acceptedClient);
                }).Start();
            }
        }
    }
}
