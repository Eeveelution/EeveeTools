using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EeveeTools.Extensions {
    public static class HttpListenerExtensions {
        /// <summary>
        /// Writes a String and Closes the Connection
        /// </summary>
        /// <param name="Response">this Object</param>
        /// <param name="String">What to Return</param>
        /// <returns>awaitable Task</returns>
        public static async Task WriteString(this HttpListenerResponse Response, string String) {
            //Convert String to byte[]
            byte[] originalString = Encoding.UTF8.GetBytes(String);
            //Write Async
            await Response.OutputStream.WriteAsync(originalString);
            //Close Connection
            Response.Close();
        }
        /// <summary>
        /// Sends a Byte Array of Data and Closes the Connection
        /// </summary>
        /// <param name="Response">this Object</param>
        /// <param name="Data">What to send Back</param>
        /// <returns>awaitable Task</returns>
        public static async Task WriteBytes(this HttpListenerResponse Response, byte[] Data) {
            //Write Async
            await Response.OutputStream.WriteAsync(Data);
            //Close Connection
            Response.Close();
        }
    }
}
