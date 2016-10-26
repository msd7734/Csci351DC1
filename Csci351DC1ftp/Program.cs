using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csci351DC1ftp
{
    class Program
    {
        static void SnipBitsTest()
        {
            // 0b10010111
            byte b = 0x97;
            Console.WriteLine(Bits.SnipBits(b, new byte[]{0, 1, 2}));
            Console.WriteLine(Bits.SnipBits(b, new byte[] { 7 }));
        }

        static void NackPacketTest()
        {
            NackPacket p = new NackPacket(1);
            byte[] bytes = p.GetBytes();
            PrintByteArr(bytes);

            Console.WriteLine(BitConverter.ToInt16(bytes, 0));
            Console.WriteLine(BitConverter.ToInt16(bytes, 2));
        }

        static void ErrPacketTest()
        {
            ErrorPacket p = new ErrorPacket(4, "Some crap went wrong.");
            byte[] bytes = p.GetBytes();
            PrintByteArr(bytes);

            Console.WriteLine(BitConverter.ToInt16(bytes, 0));
            Console.WriteLine(BitConverter.ToInt16(bytes, 2));
            Console.WriteLine(Encoding.ASCII.GetString(bytes, 4, bytes.Length - 4));
        }

        static void PrintByteArr(byte[] bytes)
        {
            foreach (byte b in bytes)
            {
                Console.Write("{0} ", Bits.ByteToBinStr(b));
            }
            Console.WriteLine();
        }

        public static void Main(string[] args)
        {
            ErrPacketTest();

            // wait to end
            Console.ReadKey(true);
        }
    }
}
