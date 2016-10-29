using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Csci351DC1ftp
{
    public class DataPacket : TFTPPacket
    {
        // Data packets contain a max of 512 bytes of data
        private static int BUF_SIZE = 512;
        // And are Hamming encoded in 32-bit (4 byte) blocks
        private static int BLK_SIZE = 4;
        // Indeces of Hamming parity bits
        private static byte[] HAMMING_BIT_INDX = { 0, 1, 3, 7, 15, 31 };

        public short BlockNum { get; private set; }

        public byte[] Data { get; private set; }

        public DataPacket(short blockNum, byte[] data)
            : base(Opcode.DATA)
        {
            BlockNum = blockNum;
            Data = data.Take(BUF_SIZE).ToArray();
        }

        // Maybe pull this somewhere else, the packet data holder probably shouldn't
        //  be responsible for handling hamming stuff (data agnosticism)
        public byte[] GetUnhammedData()
        {
            // mmm, ham...

            byte[] encBlock = new byte[BLK_SIZE];

            for (int i = 0; i < BUF_SIZE / 4; ++i)
            {
                // network bytes come in lil' Endian, but Hamming is done in big, so reverse
                encBlock = Data.Take(BLK_SIZE).Reverse().ToArray();

                foreach (byte hbit in HAMMING_BIT_INDX)
                {
                    
                }

                
            }
            // Not implementing this here. GetTruncatedData() is all this class will do operate on its own data
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get the data from the packet with trailing null bytes truncated.
        /// </summary>
        /// <returns>The truncated data as a byte array.</returns>
        public byte[] GetTruncatedData()
        {
            // starting index of null bytes
            int nullStartIndex = -1;

            for (int i = 0; i < Data.Length; ++i)
            {
                if (Data[i] == 0x0 && nullStartIndex == -1)
                {
                    nullStartIndex = i;
                }
                else if (Data[i] != 0x0)
                {
                    nullStartIndex = -1;
                }
            }

            return Data.Take(nullStartIndex).ToArray();
        }

        public override byte[] GetBytes()
        {
            byte[] opcBytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)this.Opcode));
            byte[] blknumBytes = BitConverter.GetBytes(BlockNum);
            return opcBytes.Concat(blknumBytes).Concat(Data).ToArray();
        }
    }
}
