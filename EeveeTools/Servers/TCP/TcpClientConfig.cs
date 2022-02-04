using System;

namespace EeveeTools.Servers.TCP {
    public class TcpClientConfig {
        /// <summary>
        /// How much should the Read Buffer size be, as in how many bytes should we attempt to read each time
        /// </summary>
        public int ReadBufferLimit = 8192;
        /// <summary>
        /// Ping timeout, how often pings should be sent
        /// </summary>
        public TimeSpan PingTimeout = new TimeSpan(0, 0, 0, 10);
        /// <summary>
        /// Pong timeout, after how many seconds should we disconnect the client if we don't get anything
        /// </summary>
        public TimeSpan PongTimeout = new TimeSpan(0, 0, 0, 20);
        /// <summary>
        /// Whether to use the Built in Timeouts that kill the client after a certain amount of time
        /// </summary>
        public bool ClientTimeouts = true;
        /// <summary>
        /// Whether to use the Built in CLient Pings that ping the client after a certain amount of time
        /// </summary>
        public bool ClientPings = true;
    }
}
