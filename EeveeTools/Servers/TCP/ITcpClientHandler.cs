using System;
using System.Collections.Concurrent;
using System.Net.Sockets;

namespace EeveeTools.Servers.TCP {
    public interface ITcpClientHandler {
        protected TcpClient Client { get; set; }
        protected NetworkStream ClientStream { get; set; }
        protected ConcurrentQueue<byte[]> SendQueue { get; set; }

        protected TcpClientConfig ClientConfig { get; set; }

        /// <summary>
        /// The Last Time we pinged the Client
        /// </summary>
        protected DateTime LastPingTime { get; set; }
        /// <summary>
        /// The Last Time the client got back to us
        /// </summary>
        protected DateTime LastPongTime { get; set; }

        /// <summary>
        /// Used for Initializing the client
        /// </summary>
        internal virtual void Initialize(TcpClient client, TcpClientConfig clientConfig) {
            this.Client       = client;
            this.ClientStream = client.GetStream();
            this.SendQueue    = new ConcurrentQueue<byte[]>();

            this.ClientConfig = clientConfig;
        }

        internal virtual void HandleClient() {
            //Kill Client if it times out
            if (this.LastPongTime + this.ClientConfig.PongTimeout <= DateTime.Now && this.LastPongTime != DateTime.MinValue && this.ClientConfig.ClientTimeouts) {
                this.KillClient("Client timed out.");
                return;
            }

            //Check if there's something available to read
            if (this.Client.Available != 0 && this.ClientStream.DataAvailable && this.ClientStream.CanRead) {
                //Read
                byte[] buffer = new byte[this.ClientConfig.ReadBufferLimit];
                int recieved = this.ClientStream.Read(buffer, 0, this.ClientConfig.ReadBufferLimit);

                //Cut out unnecessary part
                byte[] readBuffer = new byte[recieved];
                Buffer.BlockCopy(buffer, 0, readBuffer, 0, recieved);

                //Set the Last Pong time to now, as we've just recieved something from the client
                this.LastPongTime = DateTime.Now;

                //Handle
                this.HandleIncoming(readBuffer);
            }
        }

        protected void HandleIncoming(byte[] buffer);

        protected virtual void SendOutgoing() {
            try {
                while (this.SendQueue.TryDequeue(out byte[] toSend)) {
                    this.ClientStream.Write(toSend, 0, toSend.Length);
                    this.ClientStream.Flush();

                    this.LastPingTime = DateTime.Now;
                }

                //If we've exceeded 10 seconds without sending anything, send a ping
                if (this.LastPingTime + this.ClientConfig.PingTimeout <= DateTime.Now && this.ClientConfig.ClientPings) {
                    this.PingClient();
                    this.LastPingTime = DateTime.Now;
                }
            }
            catch(Exception e) {
                this.HandleExceptions(e);
            }
        }

        public virtual void KillClient(string reason) {
            this.Client.Close();
            this.ClientStream.Close();

            this.Client.Dispose();
            this.ClientStream.Dispose();
        }

        public virtual void PingClient() {}

        protected virtual void HandleExceptions(Exception e) {}

        protected virtual void QueueSend(byte[] buffer) {
            this.SendQueue.Enqueue(buffer);
        }
    }
}
