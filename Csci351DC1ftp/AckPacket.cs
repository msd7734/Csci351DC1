using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Csci351DC1ftp
{
    public class AckPacket : TFTPPacket
    {
        public short BlockNum { get; private set; }

        public AckPacket(short blockNum)
            : base(Opcode.ACK)
        {
            BlockNum = blockNum;
        }

        public override byte[] GetBytes()
        {
            byte[] opcBytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)this.Opcode));
            byte[] blknumBytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(BlockNum));
            return opcBytes.Concat(blknumBytes).ToArray();
        }
    }
}
