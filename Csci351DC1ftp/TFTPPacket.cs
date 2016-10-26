using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csci351DC1ftp
{
    public enum Opcode
    {
        RRQ = 1,
        RRQC = 2,
        DATA = 3,
        ACK = 4,
        ERROR = 5,
        NACK = 6
    }

    public abstract class TFTPPacket
    {
        public Opcode Opcode { get; private set; }

        public TFTPPacket(Opcode opcode)
        {
            this.Opcode = opcode;
        }

        public abstract byte[] GetBytes();

    }
}
