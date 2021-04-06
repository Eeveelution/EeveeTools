using System;
using System.Security.Cryptography;
using System.Text;

namespace EeveeTools.Helpers {
    public class CryptoHelper {
        /// <summary>
        /// Hashes a String with Sha256
        /// </summary>
        /// <param name="input">Input String</param>
        /// <returns>Returning Hex String</returns>
        public static string HashSha256(string input) {
            using SHA256 sha256 = SHA256.Create();
            byte[] byteInput = Encoding.UTF8.GetBytes(input);
            return BitConverter.ToString(sha256.ComputeHash(byteInput)).Replace("-", "").ToLower();
        }
        /// <summary>
        /// Hashes a String with MD5
        /// </summary>
        /// <param name="input">Input String</param>
        /// <returns>Returning Hex String</returns>
        public static string HashMd5(string input) {
            using MD5 sha256 = MD5.Create();
            byte[] byteInput = Encoding.UTF8.GetBytes(input);
            return BitConverter.ToString(sha256.ComputeHash(byteInput)).Replace("-", "").ToLower();
        }
    }
}
