using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using EeveeTools.Helpers;

namespace EeveeTools.Servers.TCP {
    public class WorkerTcpServer<pClientHandler> where pClientHandler : class, ITcpClientHandler, new() {
        /// <summary>
        /// Server Configuration
        /// </summary>
        internal TcpServerConfig ServerConfig;
        /// <summary>
        /// Client Configuration
        /// </summary>
        internal TcpClientConfig ClientConfig;

        /// <summary>
        /// Server's TCPListener
        /// </summary>
        private TcpListener _tcpListener;
        /// <summary>
        /// Whether the Server should continue Listening
        /// </summary>
        private bool        _continueListening;

        /// <summary>
        /// List of Client Workers
        /// </summary>
        internal List<TcpWorker<pClientHandler>> ClientWorkers;

        private List<pClientHandler> _clients;
        private object               _clientLock;

        /// <summary>
        /// Creates a Worker TCP Server
        /// </summary>
        /// <param name="serverConfiguration">Configuration of the Server</param>
        public WorkerTcpServer(TcpServerConfig serverConfiguration, TcpClientConfig clientConfiguration) {
            this.ServerConfig = serverConfiguration;
            this.ClientConfig = clientConfiguration;

            this._tcpListener = new TcpListener(IPEndPoint.Parse(this.ServerConfig.ServerLocation));
            this._clients     = new List<pClientHandler>();
            this._clientLock  = new object();

            for(int i = 0; i != this.ServerConfig.InitialClientWorkers; i++)
                this.AddWorker();
        }
        /// <summary>
        /// Runs the Server
        /// </summary>
        public void RunServer() {
            this._tcpListener.Start();

            this._continueListening = true;

            ThreadHelper.Go(() => {
                while (this._continueListening) {
                    //TODO: add worker checking and stuff
                }
            });

            while (this._continueListening) {
                TcpClient newClient = this._tcpListener.AcceptTcpClient();

                ThreadHelper.Go(() => {
                    pClientHandler client = new pClientHandler();
                    client.Initialize(newClient, this.ClientConfig);

                    if (client.Authenticate()) {
                        lock (this._clientLock) {
                            this._clients.Add(client);
                        }
                    }
                });
            }
        }
        /// <summary>
        /// Stops the Server
        /// </summary>
        public void StopServer() { this._continueListening = false; }
        /// <summary>
        /// Adds a Additional Client Worker to the Server
        /// </summary>
        public void AddWorker() {
            TcpWorker<pClientHandler> worker = new TcpWorker<pClientHandler>(this, this.ClientWorkers.Count);
            worker.Start();

            this.ClientWorkers.Add(worker);
        }
        /// <summary>
        /// Removes the latest Client worker off the server
        /// </summary>
        public void RemoveWorker() {
            TcpWorker<pClientHandler> worker = this.ClientWorkers[^1];
            worker.Decomission();

            this.ClientWorkers.Remove(worker);
        }

        public int GetWorkerCount() => this.ClientWorkers.Count;

        internal pClientHandler GetProcessableClient(TcpWorker<pClientHandler> worker) {
            try {
                lock (this._clientLock) {
                    int count = this._clients.Count;

                    if (count == 0)
                        return null;

                    //Work out what range of clients each worker should process
                    int range = (int) Math.Ceiling((float) count / this.GetWorkerCount());
                    //Determine it's start
                    int start = range * worker.Id;

                    //Get its index using the start and count, and what last client the worker processed
                    int index = Math.Min(count - 1, start + worker.LastProcessedIndex);

                    if (index < start)
                        return null;

                    pClientHandler client = this._clients[index];

                    //Increase its last processed client, so that next time this worker comes around, it will pick the next client
                    worker.LastProcessedIndex = (worker.LastProcessedIndex + 1) % range;

                    return client;
                }
            }
            catch {
                return null;
            }
        }

        private void CheckWorkerCount() {
            
        }
    }
}
