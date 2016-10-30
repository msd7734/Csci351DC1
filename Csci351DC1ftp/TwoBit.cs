using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csci351DC1ftp
{
    public class TwoBit
    {
        public byte BitOne { get; private set; }

        public byte BitTwo { get; private set; }

        public TwoBit(byte b2, byte b1)
        {
            if (b1 + b1 > 2)
                throw new ArgumentOutOfRangeException();

            BitOne = b1;
            BitTwo = b2;
        }

        public byte AsByte()
        {
            return (byte)(BitOne + (2 * BitTwo));
        }
    }
}
