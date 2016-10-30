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

        public static UInt32 NthBit(UInt32 i32, byte nbit)
        {
            if (nbit > 31 || nbit < 0)
                throw new BitOpException("The bit index provided was out of bounds for a 32-bit int.");

            return Convert.ToByte(((i32 >> nbit) & 0x01));
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
        /// Return an unsigned 32-bit int with the bits at the given indeces removed.
        /// </summary>
        /// <param name="b">The int to operate on.</param>
        /// <param name="snip">An array of indeces to snip.</param>
        /// <returns>The "snipped" int.</returns>
        public static UInt32 SnipBits(UInt32 i32, byte[] snip)
        {
            if (snip.Length == 0)
                return i32;

            uint res = 0;
            int skipped = 0;
            for (byte i = 0; i < 32; ++i)
            {
                if (snip.Contains(i))
                {
                    skipped += 1;
                }
                else
                {
                    res += Convert.ToUInt32(Math.Pow(2, i - skipped) * NthBit(i32, i));
                }

            }
            return res;
        }

        /// <summary>
        /// Return the index of the highest order "1" bit of a byte.
        /// </summary>
        /// <param name="b">The byte to check</param>
        /// <returns>A bit index (0-7). A null byte (0x0) returns 0.</returns>
        public static int HighestOrderBit(byte b)
        {
            if (b == 0x0)
                return 0;

            int i;
            for (i = -1; b != 0x0; ++i)
            {
                b = (byte)(b >> 1);
            }
            return i;
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

        public static String Int32ToBinStr(UInt32 i)
        {
            return Convert.ToString(i, 2).PadLeft(32, '0');
        }

        public static byte TwoBitsToByte(TwoBit[] bits)
        {
            if (bits.Length != 4)
                throw new ArgumentOutOfRangeException();

            return (byte)(bits[0].AsByte() + (bits[1].AsByte() * 4) + (bits[2].AsByte() * 16) + (bits[3].AsByte() * 64));
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
