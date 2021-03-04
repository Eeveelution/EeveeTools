/*using System;
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

        private readonly Action<TcpClient, byte[]> _onConnectionData
        /// <summary>
        /// Creates a TCP Server with Handlers at `Location`:`Port`
        /// </summary>
        /// <param name="Location">Where to Run</param>
        /// <param name="Port">At what Port</param>
        /// <param name="ConnectionDataHandler">Handler for Connection Data</param>
        /// <param name="ConnectionOpenHandler">Handler for a Connection Open</param>
        public TcpServer(string Location, short Port, Action<TcpClient, byte[]> ConnectionDataHandler, Action<TcpClient> ConnectionOpenHandler) {
            this._tcpListener       = new TcpListener(IPAddress.Parse(Location), Port);
            this._cancellationToken = new CancellationTokenSource();
        }

        public void Start() {
            //Start Listener
            this._tcpListener.Start();
            //Create Task for Listening
            Task.Factory.StartNew(this.HandleClients, this._cancellationToken.Token);
        }

        private async Task HandleClients() {
            try {
                TcpClient acceptedClient
            }
            catch (OperationCanceledException) {
                this._tcpListener.Stop();
            }
        }
    }
}
*/