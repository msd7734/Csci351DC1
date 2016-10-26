using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csci351DC1ftp
{
    // Hold useful methods for bit manipulation
    public static class Bits
    {
        public static byte NthBit(byte b, byte nbit)
        {
            if (nbit > 7 || nbit < 0)
                throw new BitOpException("The bit index provided was out of bounds for a byte.");

            return Convert.ToByte(((b >> nbit) & 0x01));
        }
    }

    public class BitOpException : Exception
    {
        private static string defaultMessage = "An invalid bit operation was performed.";

        public BitOpException() : base(defaultMessage) { }

        public BitOpException(string message) : base(message) { }
    }
}
