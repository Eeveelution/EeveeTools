using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
#pragma warning disable 4014

namespace EeveeTools.Servers.HTTP {
    /// <summary>
    /// Async HTTP Server which Allows for Fast Requests and Easy Usage
    /// </summary>
    public class AsyncHttpServer {
        /// <summary>
        /// Is the Server running?
        /// </summary>
        public bool IsRunning => this._listener.IsListening;
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
        private readonly Func<string, HttpListenerContext, Task> _asyncRequestHandler;
        /// <summary>
        /// Creates a Async HTTP Server on `Location` and Invokes `AsyncRequestHandler` on Request
        /// </summary>
        /// <param name="location">Where to Run the Server</param>
        /// <param name="asyncRequestHandler">What method to call upon Recieving a Request</param>
        public AsyncHttpServer(string location, Func<string, HttpListenerContext, Task> asyncRequestHandler) {
            this._asyncRequestHandler = asyncRequestHandler;

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
        /// Void for Handling Connections and Invoking the <see cref="_asyncRequestHandler"/>
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
                        //Invoke Method (NOT AWAITED FOR SPEED)
                        this._asyncRequestHandler(url, context);
                    }
                }
            }
            catch (OperationCanceledException) {
                return;
            }
        }
    }
}
