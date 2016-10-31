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

    /// <summary>
    /// Define the basics of a packet for the Hamming Server.
    /// </summary>
    public abstract class TFTPPacket
    {
        /// <summary>
        /// Get the Opcode that represents this packet.
        /// </summary>
        public Opcode Opcode { get; private set; }

        public TFTPPacket(Opcode opcode)
        {
            this.Opcode = opcode;
        }

        /// <summary>
        /// Get the raw byte representation of this packet as it would be sent across the network.
        /// </summary>
        /// <returns>An array of bytes.</returns>
        public abstract byte[] GetBytes();

    }
}
