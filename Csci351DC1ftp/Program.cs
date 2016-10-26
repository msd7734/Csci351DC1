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

        static void DataPacketTest()
        {
            DataPacket p = new DataPacket(1, new byte[] { 0xF, 0xA, 0xB, 0x0, 0xF0, 0x0, 0x0 });
            p.Data.Take(-1);
            Console.WriteLine(p.GetTruncatedData().Length);
            PrintByteArr(p.GetTruncatedData());
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
            DataPacketTest();

            // wait to end
            Console.ReadKey(true);
        }
    }
}
