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

        /// <summary>
        /// Convert an array of up to 4 bit pairs to a byte.
        /// </summary>
        /// <param name="bits">A TwoBit array whose lower indeces indicate lower order bit pairs.</param>
        /// <returns>A byte with the value of the concatenated bit pairs.</returns>
        public static byte TwoBitsToByte(TwoBit[] bits)
        {
            if (bits.Length > 4)
                throw new ArgumentOutOfRangeException();

            byte res = 0;
            for (int i = 0; i < bits.Length; ++i)
            {
                res += (byte)(bits[i].AsByte() * Math.Pow(2, i*2.0));
            }

            return res;
        }


        /// <summary>
        /// Make as many bit pairs as possible from the significant bits of a given byte.
        /// </summary>
        /// <param name="b">The byte to split into bit pairs.</param>
        /// <returns>A TwoBit array of size 0-4 (size 0 if b = 0).</returns>
        public static TwoBit[] ByteToTwoBits(byte b)
        {
            var twoBits = new List<TwoBit>(4);

            if (b == 0x0)
                return twoBits.ToArray();

            int highestBitInd = HighestOrderBit(b) + 1;
            for (int i = 0; i < highestBitInd; i += 2)
            {
                twoBits.Add(new TwoBit(b));
                b = (byte)(b >> 2);
            }

            return twoBits.ToArray();
        }

        /// <summary>
        /// Make a specified number of bit pairs from a given byte.
        /// </summary>
        /// <param name="b">The byte to split into bit pairs.</param>
        /// <param name="count">How many bit pairs to make.</param>
        /// <returns>A TwoBit array of size 0-4 (size 0 if b = 0).</returns>
        public static TwoBit[] ByteToTwoBits(byte b, int count)
        {
            var twoBits = new List<TwoBit>(4);
            for (int i = 0; i < count; ++i)
            {
                twoBits.Add(new TwoBit(b));
                b = (byte)(b >> 2);
            }
            return twoBits.ToArray();
        }

        /// <summary>
        /// Invert the bit order of a byte.
        /// </summary>
        /// <param name="b">The byte to invert.</param>
        /// <returns>The inverted byte.</returns>
        public static byte InvertBits(byte b)
        {
            // Shamelessly ~stolen~ (sourced) from here:
            // http://graphics.stanford.edu/~seander/bithacks.html#ReverseByteWith64BitsDiv
            ulong u = ((b * 0x0202020202UL) & 0x010884422010UL) % 1023;
            return (byte)u;
        }

        /// <summary>
        /// Check if a 32-bit sequence has an even number of 1's.
        /// </summary>
        /// <param name="binSeq">The sequence to check, represented as a UInt32.</param>
        /// <returns>True if the sequence has an even # of 1's, otherwise false.</returns>
        public static bool HasEvenOnes(uint binSeq)
        {
            int ones = 0;
            uint mask = 0x1;

            for (int i = 0; i < 32; ++i)
            {
                if ((binSeq >> i & mask) == 0x1)
                    ones += 1;
            }

            return ones % 2 == 0;
        }

        /// <summary>
        /// Flip the nth bit of a 32-bit sequence.
        /// </summary>
        /// <param name="binSeq">The 32-bit sequence to operate on.</param>
        /// <param name="n">The index (0-31) of the bit to flip.</param>
        /// <returns>The 32-bit sequence with the chosen bit flipped.</returns>
        public static uint FlipNthBit(uint binSeq, byte n)
        {
            if (n > 31 || n < 0)
                return binSeq;

            uint mask = (uint)(0x01 << n);
            return binSeq ^ mask;
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
