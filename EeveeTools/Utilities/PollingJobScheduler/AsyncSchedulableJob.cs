using System;
using System.Threading.Tasks;

namespace EeveeTools.Utilities.PollingJobScheduler {
    public abstract class AsyncSchedulableJob {
        public DateTime NextExecutionTime = DateTime.MinValue;
        /// <summary>
        /// Time between each execution of the job. (In milliseconds
        /// </summary>
        public abstract TimeSpan ExecuteTimeout { get; }
        /// <summary>
        /// Method that gets called every <see cref="ExecuteTimeout"/>
        /// </summary>
        /// <returns></returns>
        public abstract Task ExecuteJob();
    }
}
