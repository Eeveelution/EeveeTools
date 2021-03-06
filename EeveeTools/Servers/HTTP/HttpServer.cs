using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
#pragma warning disable 4014

namespace EeveeTools.Servers.HTTP {
    /// <summary>
    /// HTTP Server which Allows for Fast Requests and Easy Usage
    /// </summary>
    public class HttpServer {
        /// <summary>
        /// Http Listener, Runs the Server
        /// </summary>
        private readonly HttpListener            _listener;
        /// <summary>
        /// Cancellation Token for Stopping the Server
        /// </summary>
        private readonly CancellationTokenSource _cancellationToken;
        /// <summary>
        /// Method that is Invoked on a Request
        /// </summary>
        private readonly Action<string, HttpListenerContext> _requestHandler;
        /// <summary>
        /// Sees if the Server has been started and is listening for connection
        /// </summary>
        public bool IsRunning => this._listener.IsListening;

        /// <summary>
        /// Creates a Async HTTP Server on `Location` and Invokes `AsyncRequestHandler` on Request
        /// </summary>
        /// <param name="location">Where to Run the Server</param>
        /// <param name="requestHandler">What method to call upon Recieving a Request</param>
        public HttpServer(string location, Action<string, HttpListenerContext> requestHandler) {
            this._requestHandler = requestHandler;

            this._cancellationToken = new CancellationTokenSource();

            this._listener = new HttpListener();
            this._listener.Prefixes.Add(location);
            this._listener.Start();
        }
        /// <summary>
        /// Creates the Handle Connections Task
        /// </summary>
        public void Start() {
            Task.Factory.StartNew(this.HandleConnections, this._cancellationToken.Token);
        }
        /// <summary>
        /// Stops the Server
        /// </summary>
        public void Stop() {
            this._cancellationToken.Cancel();
        }
        /// <summary>
        /// Void for Handling Connections and Invoking the `_asyncRequestHandler`
        /// </summary>
        /// <returns>Async Task</returns>
        private async Task HandleConnections() {
            try {
                while (!this._cancellationToken.IsCancellationRequested) {
                    //Grab Context
                    HttpListenerContext context = await this._listener.GetContextAsync();
                    //Check for Null
                    if (context.Request.Url is not null) {
                        //Get URL
                        string url = new Uri(context.Request.Url.OriginalString).AbsolutePath;
                        //Invoke Method
                        this._requestHandler(url, context);
                    }
                }
            }
            catch (OperationCanceledException) {
                return;
            }
        }
    }
}
