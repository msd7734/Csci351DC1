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
            // 0b10010111
            byte b = 0x97;
            Console.WriteLine(Bits.SnipBits(b, new byte[]{0, 1, 2}));
            Console.WriteLine(Bits.SnipBits(b, new byte[] { 7 }));
            
            // wait to end
            Console.ReadKey(true);
        }
    }
}
