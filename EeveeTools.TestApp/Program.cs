using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EeveeTools.Database;
using EeveeTools.Extensions;
using EeveeTools.Servers.HTTP;
using EeveeTools.Servers.TCP;
using EeveeTools.Utilities.PollingJobScheduler;
using MySqlConnector;

namespace EeveeTools.TestApp {
    class Program {
        private class TestJob : AsyncSchedulableJob {
            public override TimeSpan ExecuteTimeout { get; } = new TimeSpan(0, 0, 0, 5);
            public override async Task ExecuteJob() {
                Console.WriteLine("hi");
            }
        }

        static async Task Main(string[] args) {
            AsyncPollingScheduler scheduler = new();
            scheduler.AddJob(new TestJob());

            new Thread(() => {Thread.Sleep(10000); scheduler.QueueMicrotask(() => {Console.WriteLine("microtask");});}).Start();
            new Thread(() => {
                Thread.Sleep(12000);
                scheduler.QueueMicrotask(delegate { Console.WriteLine("microtask"); });
            }).Start();

            await scheduler.RunScheduler();
        }
    }
}
