using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csci351DC1ftp
{
    public class NackPacket : TFTPPacket
    {
        public short BlockNum { get; private set; }

        public NackPacket(short blockNum) : base(Opcode.NACK)
        {
            BlockNum = blockNum;
        }

        public override byte[] GetBytes()
        {
            byte[] opcBytes = BitConverter.GetBytes((short)this.Opcode);
            byte[] blknumBytes = BitConverter.GetBytes(BlockNum);
            return opcBytes.Concat(blknumBytes).ToArray();
        }
    }
}
