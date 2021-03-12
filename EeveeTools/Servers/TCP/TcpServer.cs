using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace EeveeTools.Servers.TCP {
    public class TcpServer {
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
        /// Creates a TCP Server with Handlers at `Location`:`Port`
        /// </summary>
        /// <param name="Location">Where to Run</param>
        /// <param name="Port">At what Port</param>
        /// <param name="ConnectionDataHandler">Handler for Connection Data</param>
        /// <param name="ClientHandlerType">Handler for a Connection Open</param>
        public TcpServer(string Location, short Port, Type ClientHandlerType) {
            this._tcpListener       = new TcpListener(IPAddress.Parse(Location), Port);
            this._cancellationToken = new CancellationTokenSource();
            this._clientHandlerType = ClientHandlerType;
            //Check if Type is valid
            if (!ClientHandlerType.IsAssignableFrom(typeof(TcpClientHandler))) {
                //throw new Exception("You need to pass a class that is inherited by ClientHandler..");
            }
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
                new Thread(() => { connectionHandler?.HandleNew(acceptedClient); }).Start();
            }
        }
    }
}
