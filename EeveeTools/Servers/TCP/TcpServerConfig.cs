namespace EeveeTools.Servers.TCP {
    public class TcpServerConfig {
        /// <summary>
        /// Max Amount of Client Workers
        /// </summary>
        public int MaxClientWorkers       = 2;
        /// <summary>
        /// How many times are the Clients supposed to get Updated
        /// </summary>
        public int ClientUpdatesPerSecond = 10;
        /// <summary>
        /// How many Clients is a Worker supposed to be processing
        /// </summary>
        public int WorkerClientsPerSecond = 10000;

        /// <summary>
        /// Location of the Server, i.e 127.0.0.1:13381
        /// </summary>
        public string ServerLocation;
    }
}
