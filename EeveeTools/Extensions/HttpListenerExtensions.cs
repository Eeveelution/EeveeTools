using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EeveeTools.Extensions {
    public static class HttpListenerExtensions {
        /// <summary>
        /// Writes a String and Closes the Connection
        /// </summary>
        /// <param name="response">this Object</param>
        /// <param name="string">What to Return</param>
        /// <returns>awaitable Task</returns>
        public static async Task WriteStringAsync(this HttpListenerResponse response, string @string) {
            //Convert String to byte[]
            byte[] originalString = Encoding.UTF8.GetBytes(@string);
            //Write Async
            await response.OutputStream.WriteAsync(originalString);
            //Close Connection
            response.Close();
        }
        /// <summary>
        /// Writes a String and Closes the Connection
        /// </summary>
        /// <param name="response">this Object</param>
        /// <param name="string">What to Return</param>
        /// <returns>awaitable Task</returns>
        public static void WriteString(this HttpListenerResponse response, string @string) {
            //Convert String to byte[]
            byte[] originalString = Encoding.UTF8.GetBytes(@string);
            //Write Async
            response.OutputStream.Write(originalString);
            //Close Connection
            response.Close();
        }
        /// <summary>
        /// Sends a Byte Array of Data and Closes the Connection
        /// </summary>
        /// <param name="response">this Object</param>
        /// <param name="data">What to send Back</param>
        /// <returns>awaitable Task</returns>
        public static async Task WriteBytesAsync(this HttpListenerResponse response, byte[] data) {
            //Write Async
            await response.OutputStream.WriteAsync(data);
            //Close Connection
            response.Close();
        }
        /// <summary>
        /// Sends a Byte Array of Data and Closes the Connection
        /// </summary>
        /// <param name="response">this Object</param>
        /// <param name="data">What to send Back</param>
        public static void WriteBytes(this HttpListenerResponse response, byte[] data) {
            //Write
            response.OutputStream.Write(data);
            //Close Connection
            response.Close();
        }
    }
}
