using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csci351DC1ftp
{
    /// <summary>
    /// Encapsulates bit pairs.
    /// </summary>
    public class TwoBit
    {
        public byte BitOne { get; private set; }

        public byte BitTwo { get; private set; }

        /// <summary>
        /// Construct a TwoBit from two bytes that equal 0 or 1.
        /// </summary>
        /// <param name="b2">The higher order bit</param>
        /// <param name="b1">The lower order bit</param>
        public TwoBit(byte b2, byte b1)
        {
            if (b1 + b1 > 2)
                throw new ArgumentOutOfRangeException();

            BitOne = b1;
            BitTwo = b2;
        }

        /// <summary>
        /// Construct a TwoBit from the trailing two bits of a byte.
        /// </summary>
        /// <param name="b">The byte whose trailing bits to use.</param>
        public TwoBit(byte b)
        {
            BitOne = Bits.NthBit(b, 0);
            BitTwo = Bits.NthBit(b, 1);
        }

        public void Invert()
        {
            byte temp = BitOne;
            BitOne = BitTwo;
            BitTwo = temp;
        }

        public byte AsByte()
        {
            return (byte)(BitOne + (2 * BitTwo));
        }
    }
}
