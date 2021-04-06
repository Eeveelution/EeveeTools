using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EeveeTools.Helpers {
    /// <summary>
    /// BinaryWriter with PeppyUleb128 writing
    /// </summary>
    public class BanchoWriter : BinaryWriter {
        public BanchoWriter(Stream readStream) : base(readStream, Encoding.Default, true) {}
        public BanchoWriter(Stream readStream, Encoding encoding, bool close) : base(readStream, encoding, close) {}
        /// <summary>
        /// Writes a String
        /// </summary>
        /// <param name="value">String</param>
        public override void Write(string value) {
            this.Write(Uleb128.WriteString(value));
        }
    }
    /// <summary>
    /// BinaryReader with PeppyUleb128 reading
    /// </summary>
    public class BanchoReader : BinaryReader {
        public BanchoReader(Stream readStream) : base(readStream, Encoding.Default, true) {}
        public BanchoReader(Stream readStream, Encoding encoding, bool close) : base(readStream, encoding, close) {}
        /// <summary>
        /// Reads a String
        /// </summary>
        /// <returns>Read String</returns>
        public override string ReadString() {
            return Uleb128.ReadString(this.BaseStream);
        }
    }
    public static class Uleb128 {
        /// <summary>
        /// Writes Uleb128 Length
        /// </summary>
        /// <param name="num">Length</param>
        /// <returns>Length array</returns>
        private static byte[] Write_Uleb128(int num) {
            List<byte> ret = new();

            if (num == 0) {
                return new byte[] { 0x00 };
            }

            int length = 0;

            while (num > 0) {
                ret.Add((byte) (num & 127));
                num >>= 7;
                if (num != 0) {
                    ret[length] |= 128;
                }
                length += 1;
            }

            return ret.ToArray();
        }
        /// <summary>
        /// Writes a String
        /// </summary>
        /// <param name="s">String</param>
        /// <returns>Buffer with Uleb128 String</returns>
        public static byte[] WriteString(string s) {

            if (s.Length == 0)
                return new byte[] { 0x00 };
            List<byte> ret = new();

            ret.Add(11);
            ret.AddRange(Write_Uleb128(s.Length));
            ret.AddRange(Encoding.UTF8.GetBytes(s));

            return ret.ToArray();
        }
        /// <summary>
        /// Reads a String from a Stream
        /// </summary>
        /// <param name="s">Stream</param>
        /// <returns>String</returns>
        public static string ReadString(Stream s) {
            using BinaryReader reader = new(s, Encoding.Default, true);
            byte type = reader.ReadByte();
            if (type == 11)
                return Encoding.UTF8.GetString(Encoding.ASCII.GetBytes(reader.ReadString()));
            else return "";
        }
    }
}
