using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csci351DC1ftp
{
    /// <summary>
    /// Hold useful methods for bit manipulation.
    /// </summary>
    public static class Bits
    {
        /// <summary>
        /// Get the nth bit of a byte.
        /// </summary>
        /// <param name="b">The byte to operate on.</param>
        /// <param name="nbit">The index of the bit to return, where index 0 is the rightmost bit and 7 is the leftmost.</param>
        /// <returns>A byte of value 0 or 1.</returns>
        public static byte NthBit(byte b, byte nbit)
        {
            if (nbit > 7 || nbit < 0)
                throw new BitOpException("The bit index provided was out of bounds for a byte.");

            return Convert.ToByte(((b >> nbit) & 0x01));
        }

        /// <summary>
        /// Return a byte with the bits at the given indeces removed.
        /// </summary>
        /// <param name="b">The byte to operate on.</param>
        /// <param name="snip">An array of indeces to snip.</param>
        /// <returns>The "snipped" byte.</returns>
        public static byte SnipBits(byte b, byte[] snip)
        {
            if (snip.Length == 0)
                return b;

            byte res = 0;
            int skipped = 0;
            for (byte i = 0; i < 8; ++i)
            {
                if (snip.Contains(i))
                {
                    skipped += 1;
                }
                else
                {
                    res += Convert.ToByte(Math.Pow(2, i - skipped) * NthBit(b, i));
                }
                
            }
            return res;
        }

        /// <summary>
        /// Convert a byte to a fixed-length binary string representation (used for testing).
        /// </summary>
        /// <param name="b">The byte to convert.</param>
        /// <returns>A binary string representation of b.</returns>
        public static string ByteToBinStr(byte b)
        {
            return Convert.ToString(b, 2).PadLeft(8, '0');
        }
    }

    /// <summary>
    /// Exception class used for all operations in the static Bits class.
    /// </summary>
    public class BitOpException : Exception
    {
        private static readonly string defaultMessage = "An invalid bit operation was performed.";

        public BitOpException() : base(defaultMessage) { }

        public BitOpException(string message) : base(message) { }
    }
}
