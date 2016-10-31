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
            uint res = Bits.NthBit(binSeq, 0);
            res ^= Bits.NthBit(binSeq, 2) << 1;
            res ^= Bits.NthBit(binSeq, 4) << 2;
            res ^= Bits.NthBit(binSeq, 6) << 3;
            res ^= Bits.NthBit(binSeq, 8) << 4;
            res ^= Bits.NthBit(binSeq, 10) << 5;
            res ^= Bits.NthBit(binSeq, 12) << 6;
            res ^= Bits.NthBit(binSeq, 14) << 7;
            res ^= Bits.NthBit(binSeq, 16) << 8;
            res ^= Bits.NthBit(binSeq, 18) << 9;
            res ^= Bits.NthBit(binSeq, 20) << 10;
            res ^= Bits.NthBit(binSeq, 22) << 11;
            res ^= Bits.NthBit(binSeq, 24) << 12;
            res ^= Bits.NthBit(binSeq, 26) << 13;
            res ^= Bits.NthBit(binSeq, 28) << 14;
            res ^= Bits.NthBit(binSeq, 30) << 15;
            return res;
        }

        public static uint HammingCode2(uint binSeq)
        {
            uint res = Bits.NthBit(binSeq, 1);
            res ^= Bits.NthBit(binSeq, 2) << 1;
            res ^= Bits.NthBit(binSeq, 5) << 2;
            res ^= Bits.NthBit(binSeq, 6) << 3;
            res ^= Bits.NthBit(binSeq, 9) << 4;
            res ^= Bits.NthBit(binSeq, 10) << 5;
            res ^= Bits.NthBit(binSeq, 13) << 6;
            res ^= Bits.NthBit(binSeq, 14) << 7;
            res ^= Bits.NthBit(binSeq, 17) << 8;
            res ^= Bits.NthBit(binSeq, 18) << 9;
            res ^= Bits.NthBit(binSeq, 21) << 10;
            res ^= Bits.NthBit(binSeq, 22) << 11;
            res ^= Bits.NthBit(binSeq, 25) << 12;
            res ^= Bits.NthBit(binSeq, 26) << 13;
            res ^= Bits.NthBit(binSeq, 29) << 14;
            res ^= Bits.NthBit(binSeq, 30) << 15;
            return res;

            /*
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
             * */
        }

        public static uint HammingCode4(uint binSeq)
        {
            uint res = Bits.NthBit(binSeq, 3);
            res ^= Bits.NthBit(binSeq, 4) << 1;
            res ^= Bits.NthBit(binSeq, 5) << 2;
            res ^= Bits.NthBit(binSeq, 6) << 3;
            res ^= Bits.NthBit(binSeq, 11) << 4;
            res ^= Bits.NthBit(binSeq, 12) << 5;
            res ^= Bits.NthBit(binSeq, 13) << 6;
            res ^= Bits.NthBit(binSeq, 14) << 7;
            res ^= Bits.NthBit(binSeq, 19) << 8;
            res ^= Bits.NthBit(binSeq, 20) << 9;
            res ^= Bits.NthBit(binSeq, 21) << 10;
            res ^= Bits.NthBit(binSeq, 22) << 11;
            res ^= Bits.NthBit(binSeq, 27) << 12;
            res ^= Bits.NthBit(binSeq, 28) << 13;
            res ^= Bits.NthBit(binSeq, 29) << 14;
            res ^= Bits.NthBit(binSeq, 30) << 15;
            return res;

            /*
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
             * */
        }

        public static uint HammingCode8(uint binSeq)
        {
            uint res = Bits.NthBit(binSeq, 7);
            res ^= Bits.NthBit(binSeq, 8) << 1;
            res ^= Bits.NthBit(binSeq, 9) << 2;
            res ^= Bits.NthBit(binSeq, 10) << 3;
            res ^= Bits.NthBit(binSeq, 11) << 4;
            res ^= Bits.NthBit(binSeq, 12) << 5;
            res ^= Bits.NthBit(binSeq, 13) << 6;
            res ^= Bits.NthBit(binSeq, 14) << 7;
            res ^= Bits.NthBit(binSeq, 23) << 8;
            res ^= Bits.NthBit(binSeq, 24) << 9;
            res ^= Bits.NthBit(binSeq, 25) << 10;
            res ^= Bits.NthBit(binSeq, 26) << 11;
            res ^= Bits.NthBit(binSeq, 27) << 12;
            res ^= Bits.NthBit(binSeq, 28) << 13;
            res ^= Bits.NthBit(binSeq, 29) << 14;
            res ^= Bits.NthBit(binSeq, 30) << 15;
            return res;

            /*
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
             * */
        }

        public static uint HammingCode16(uint binSeq)
        {
            uint res = Bits.NthBit(binSeq, 15);
            res ^= Bits.NthBit(binSeq, 16) << 1;
            res ^= Bits.NthBit(binSeq, 17) << 2;
            res ^= Bits.NthBit(binSeq, 18) << 3;
            res ^= Bits.NthBit(binSeq, 19) << 4;
            res ^= Bits.NthBit(binSeq, 20) << 5;
            res ^= Bits.NthBit(binSeq, 21) << 6;
            res ^= Bits.NthBit(binSeq, 22) << 7;
            res ^= Bits.NthBit(binSeq, 23) << 8;
            res ^= Bits.NthBit(binSeq, 24) << 9;
            res ^= Bits.NthBit(binSeq, 25) << 10;
            res ^= Bits.NthBit(binSeq, 26) << 11;
            res ^= Bits.NthBit(binSeq, 27) << 12;
            res ^= Bits.NthBit(binSeq, 28) << 13;
            res ^= Bits.NthBit(binSeq, 29) << 14;
            res ^= Bits.NthBit(binSeq, 30) << 15;
            return res;

            /*
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
             * */
        }

        public static uint HammingCode32(uint binSeq)
        {
            // This won't be used in HammingClient since bit 32 is an OVERALL parity bit
            //  for the entire 32-bit block as received
            return 0;
        }
    }
}
