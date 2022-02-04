using System;
using System.Threading;

namespace EeveeTools.Servers.TCP {
    public class TcpWorker<pClientHandler> where pClientHandler : class, ITcpClientHandler, new() {
        internal int Id;

        internal int      LastProcessedIndex;
        internal DateTime LastClientHandleRequest;

        private Thread _workerThread;
        private bool   _continueWorking;

        private WorkerTcpServer<pClientHandler> _belongingServer;

        internal TcpWorker(WorkerTcpServer<pClientHandler> server, int id) {
            this.Id                      = id;
            this.LastClientHandleRequest = DateTime.MinValue;
            this._belongingServer        = server;
        }

        public void Start() {
            this._continueWorking = true;
            this._workerThread.Start();
        }

        public void Decomission() {
            this._continueWorking = false;
            this._workerThread.Join();
        }

        private void Work() {
            while (this._continueWorking) {
                pClientHandler client = this._belongingServer.GetProcessableClient(this);
                this.LastClientHandleRequest = DateTime.Now;

                client?.HandleClient();
            }
        }
    }
}
