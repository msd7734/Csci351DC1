using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csci351DC1ftp
{
    class Program
    {
        public static void Main(string[] args)
        {
            /*
            // 0b10010111
            byte b = 0x97;
            Console.WriteLine(Bits.SnipBits(b, new byte[]{0, 1, 2}));
            Console.WriteLine(Bits.SnipBits(b, new byte[] { 7 }));
            */

            NackPacket p = new NackPacket(1);
            byte[] bytes = p.GetBytes();

            foreach (byte b in bytes)
            {
                Console.Write("{0} ", Bits.ByteToBinStr(b));
            }

            Console.WriteLine();
            Console.WriteLine(BitConverter.ToInt16(bytes, 0));
            Console.WriteLine(BitConverter.ToInt16(bytes, 2));
             
            // wait to end
            Console.ReadKey(true);
        }
    }
}
