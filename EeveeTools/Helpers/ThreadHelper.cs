using System;
using System.Threading;

namespace EeveeTools.Helpers {
    public class ThreadHelper {
        public static void Go(ThreadStart start) {
            new Thread(start).Start();
        }
    }
}
