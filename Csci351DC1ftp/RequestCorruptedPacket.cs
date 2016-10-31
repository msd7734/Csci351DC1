using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Csci351DC1ftp
{
    /// <summary>
    /// An outgoing packet to request a file from the server with intentional bit errors.
    /// </summary>
    public class RequestCorruptedPacket : TFTPPacket
    {
        // Only need to worry about one Transfer Mode apparently
        private static byte[] TRANSFER_MODE_BYTES = Encoding.ASCII.GetBytes("octet");

        /// <summary>
        /// The file to request.
        /// </summary>
        public string FileName { get; private set; }

        public RequestCorruptedPacket(string fileName)
            : base(Opcode.RRQC)
        {
            FileName = fileName;
        }

        public override byte[] GetBytes()
        {
            byte[] opcBytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)this.Opcode));
            byte[] fnameBytes = Encoding.ASCII.GetBytes(FileName);
            byte[] nul = { 0x0 };

            return opcBytes.Concat(fnameBytes)
                .Concat(nul)
                .Concat(TRANSFER_MODE_BYTES)
                .Concat(nul)
                .ToArray();
        }
    }
}
