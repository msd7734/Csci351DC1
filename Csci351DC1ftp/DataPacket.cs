/*
 * By: Matthew Dennis (msd7734)
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Csci351DC1ftp
{
    /// <summary>
    /// An incoming packet containing file data.
    /// </summary>
    public class DataPacket : TFTPPacket
    {
        // Data packets contain a max of 512 bytes of data
        private static int BUF_SIZE = 512;
        // And are Hamming encoded in 32-bit (4 byte) blocks
        private static int BLK_SIZE = 4;
        // Indeces of Hamming parity bits
        private static byte[] HAMMING_BIT_INDX = { 0, 1, 3, 7, 15, 31 };

        public short BlockNum { get; private set; }

        /// <summary>
        /// The up to 512 byte data sector of this data packet.
        /// </summary>
        public byte[] Data { get; private set; }

        public DataPacket(short blockNum, byte[] data)
            : base(Opcode.DATA)
        {
            BlockNum = blockNum;
            Data = data.Take(BUF_SIZE).ToArray();
        }

        public override byte[] GetBytes()
        {
            byte[] opcBytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)this.Opcode));
            byte[] blknumBytes = BitConverter.GetBytes(BlockNum);
            return opcBytes.Concat(blknumBytes).Concat(Data).ToArray();
        }
    }
}
