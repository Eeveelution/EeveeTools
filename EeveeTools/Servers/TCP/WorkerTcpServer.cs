using System.Net;
using System.Net.Sockets;
using EeveeTools.Helpers;

namespace EeveeTools.Servers.TCP {
    public class WorkerTcpServer<pClientHandler> where pClientHandler : ITcpClientHandler, new() {
        internal TcpServerConfig _serverConfig;

        private TcpListener _tcpListener;
        private bool        _continueListening;

        public WorkerTcpServer(TcpServerConfig configuration) {
            this._serverConfig = configuration;

            this._tcpListener = new TcpListener(IPEndPoint.Parse(configuration.ServerLocation));
        }

        public void RunServer() {
            this._tcpListener.Start();

            this._continueListening = true;

            while (this._continueListening) {
                TcpClient newClient = this._tcpListener.AcceptTcpClient();

                ThreadHelper.Go(() => {

                });
            }
        }

        public void StopServer() { this._continueListening = false; }
    }
}
