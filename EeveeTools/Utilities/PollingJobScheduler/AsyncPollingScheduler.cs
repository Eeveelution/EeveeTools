using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#pragma warning disable 4014

namespace EeveeTools.Utilities.PollingJobScheduler {
    public class AsyncPollingScheduler {
        /// <summary>
        /// Amount of Milliseconds between each Poll
        /// </summary>
        private int _pollingInterval;
        /// <summary>
        /// Used in <see cref="RunScheduler"/> to prevent using <code>while(true) { ... }</code> to give more flexibility
        /// </summary>
        private bool _continuePolling = true;
        /// <summary>
        /// List of Jobs
        /// </summary>
        private readonly List<AsyncSchedulableJob> _jobs = new();
        /// <summary>
        /// List of Microtasks to execute next Cycle
        /// </summary>
        private readonly BlockingCollection<Delegates.VoidDelegate> _tasks = new();
        /// <summary>
        /// Job Scheduler which Polls Events to execute (Microtasks) and Jobs to Execute
        /// </summary>
        /// <param name="pollingInterval"></param>
        public AsyncPollingScheduler(int pollingInterval = 1000) {
            this._pollingInterval = pollingInterval;
        }
        /// <summary>
        /// Queues a Microtask to be run next Cycle
        /// </summary>
        /// <param name="task">Task to be Run</param>
        public void QueueMicrotask(Delegates.VoidDelegate task) {
            this._tasks.Add(task);
        }
        /// <summary>
        /// Adds a Job to the Scheduler
        /// </summary>
        /// <param name="job">Schedulable Job</param>
        public void AddJob(AsyncSchedulableJob job) => this._jobs.Add(job);
        /// <summary>
        /// Runs the Scheduler
        /// </summary>
        /// <returns>awaitable Task (runs until Scheduler gets stopped)</returns>
        public async Task RunScheduler() {
            while (this._continuePolling) {
                await Task.Delay(this._pollingInterval);

                if(this._tasks.Count > 0)
                    foreach (Delegates.VoidDelegate task in this._tasks.GetConsumingEnumerable()) {
                        Task.Run(() => task.Invoke());
                    }

                if (this._jobs.Count != 0) {
                    foreach (AsyncSchedulableJob job in this._jobs.Where(job => DateTime.Now > job.NextExecutionTime)) {
                        if (job.NextExecutionTime != DateTime.MinValue) {
                            Task.Run(async () => { await job.ExecuteJob(); });
                        }
                        job.NextExecutionTime = DateTime.Now + job.ExecuteTimeout;
                    }
                }
            }
        }
        /// <summary>
        /// Stops the Scheduler from continuing execution
        /// </summary>
        public void Stop() => this._continuePolling = false;
    }
}
