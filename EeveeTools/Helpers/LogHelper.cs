using System;

namespace EeveeTools.Helpers {
    public class LogHelper {
        public static void Information(string format, params object[] args) => Console.WriteLine($"[Info] {format}", args);
        public static void Warning(string format, params object[] args) => Console.WriteLine($"[Warning] {format}", args);
        public static void Error(string format, params object[] args) => Console.WriteLine($"[ERROR] {format}", args);
        public static void Runtime(string format, params object[] args) => Console.WriteLine($"[Runtime] {format}", args);
    }
}
