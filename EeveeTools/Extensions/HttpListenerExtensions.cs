using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EeveeTools.Extensions {
    public static class HttpListenerExtensions {
        public static async Task WriteString(this HttpListenerResponse Response, string String) {
            //Convert String to byte[]
            byte[] originalString = Encoding.UTF8.GetBytes(String);
            //Write Async
            await Response.OutputStream.WriteAsync(originalString);
        }
    }
}
