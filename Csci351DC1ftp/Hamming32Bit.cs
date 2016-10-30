using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csci351DC1ftp
{
    /// <summary>
    /// Hold useful methods for interacting with Hamming encoding and 32 bit integers.
    /// </summary>
    public static class Hamming32Bit
    {
        public static bool IsValidHamming(uint hammingCode, byte parityBit)
        {
            bool evenOnes = Bits.HasEvenOnes(hammingCode);
            return (evenOnes && parityBit == 0x0) || (!evenOnes && parityBit == 0x1);
        }

        public static uint HammingCode1(uint binSeq)
        {
            return
                Bits.NthBit(binSeq, 0) +
                Bits.NthBit(binSeq, 2) * 2 +
                Bits.NthBit(binSeq, 4) * 4 +
                Bits.NthBit(binSeq, 6) * 8 +
                Bits.NthBit(binSeq, 8) * 16 +
                Bits.NthBit(binSeq, 10) * 32 +
                Bits.NthBit(binSeq, 12) * 64 +
                Bits.NthBit(binSeq, 14) * 128 +
                Bits.NthBit(binSeq, 16) * 256 +
                Bits.NthBit(binSeq, 18) * 512 +
                Bits.NthBit(binSeq, 20) * 1024 +
                Bits.NthBit(binSeq, 22) * 2048 +
                Bits.NthBit(binSeq, 24) * 4096;
        }

        public static uint HammingCode2(uint binSeq)
        {
            return
                Bits.NthBit(binSeq, 1) +
                Bits.NthBit(binSeq, 2) * 2 +
                Bits.NthBit(binSeq, 5) * 4 +
                Bits.NthBit(binSeq, 6) * 8 +
                Bits.NthBit(binSeq, 9) * 16 +
                Bits.NthBit(binSeq, 10) * 32 +
                Bits.NthBit(binSeq, 13) * 64 +
                Bits.NthBit(binSeq, 14) * 128 +
                Bits.NthBit(binSeq, 17) * 256 +
                Bits.NthBit(binSeq, 18) * 512 +
                Bits.NthBit(binSeq, 21) * 1024 +
                Bits.NthBit(binSeq, 22) * 2048 +
                Bits.NthBit(binSeq, 25) * 4096;
        }

        public static uint HammingCode4(uint binSeq)
        {
            return
                Bits.NthBit(binSeq, 3) +
                Bits.NthBit(binSeq, 4) * 2 +
                Bits.NthBit(binSeq, 5) * 4 +
                Bits.NthBit(binSeq, 6) * 8 +
                Bits.NthBit(binSeq, 11) * 16 +
                Bits.NthBit(binSeq, 12) * 32 +
                Bits.NthBit(binSeq, 13) * 64 +
                Bits.NthBit(binSeq, 14) * 128 +
                Bits.NthBit(binSeq, 19) * 256 +
                Bits.NthBit(binSeq, 20) * 512 +
                Bits.NthBit(binSeq, 21) * 1024 +
                Bits.NthBit(binSeq, 22) * 2048;
        }

        public static uint HammingCode8(uint binSeq)
        {
            return
                Bits.NthBit(binSeq, 7) +
                Bits.NthBit(binSeq, 8) * 2 +
                Bits.NthBit(binSeq, 9) * 4 +
                Bits.NthBit(binSeq, 10) * 8 +
                Bits.NthBit(binSeq, 11) * 16 +
                Bits.NthBit(binSeq, 12) * 32 +
                Bits.NthBit(binSeq, 13) * 64 +
                Bits.NthBit(binSeq, 14) * 128 +
                Bits.NthBit(binSeq, 23) * 256 +
                Bits.NthBit(binSeq, 24) * 512 +
                Bits.NthBit(binSeq, 25) * 1024;
        }

        public static uint HammingCode16(uint binSeq)
        {
            return
                Bits.NthBit(binSeq, 15) +
                Bits.NthBit(binSeq, 16) * 2 +
                Bits.NthBit(binSeq, 17) * 4 +
                Bits.NthBit(binSeq, 18) * 8 +
                Bits.NthBit(binSeq, 19) * 16 +
                Bits.NthBit(binSeq, 20) * 32 +
                Bits.NthBit(binSeq, 21) * 64 +
                Bits.NthBit(binSeq, 22) * 128 +
                Bits.NthBit(binSeq, 23) * 256 +
                Bits.NthBit(binSeq, 24) * 512 +
                Bits.NthBit(binSeq, 25) * 1024;
        }

        public static uint HammingCode32(uint binSeq)
        {
            return 0;
        }
    }
}
