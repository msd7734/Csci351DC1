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
            byte[] b1 = BitConverter.GetBytes((short)Opcode);
            byte[] b2 = BitConverter.GetBytes(BlockNum);
            return b1.Concat(b2).ToArray();
        }
    }
}
