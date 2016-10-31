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
    /// And outgoing packet to signal there was an uncorrectable error in the last data packet.
    /// </summary>
    public class NackPacket : TFTPPacket
    {
        public short BlockNum { get; private set; }

        public NackPacket(short blockNum)
            : base(Opcode.NACK)
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
