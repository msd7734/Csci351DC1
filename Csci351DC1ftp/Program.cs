using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace Csci351DC1ftp
{
    class Program
    {
        static void SnipBitsTestByte()
        {
            // 0b10010111
            byte b = 0x97;
            Console.WriteLine(Bits.SnipBits(b, new byte[]{0, 1, 2}));
            Console.WriteLine(Bits.SnipBits(b, new byte[] { 7 }));
        }

        static void SnipBitsTestInt32()
        {
            uint i = 0xA01297;
            Console.WriteLine(Bits.Int32ToBinStr(i));
            Console.WriteLine(Bits.Int32ToBinStr(Bits.SnipBits(i, new byte[] { 0, 1, 3, 7, 15, 31 })));
        }

        static void HighestOrderBitTest()
        {
            byte b1 = 0x10;
            byte b2 = 0x01;
            byte b3 = 0xF0;
            Console.WriteLine("{0} : {1}", Bits.ByteToBinStr(b1), Bits.HighestOrderBit(b1));
            Console.WriteLine("{0} : {1}", Bits.ByteToBinStr(b2), Bits.HighestOrderBit(b2));
            Console.WriteLine("{0} : {1}", Bits.ByteToBinStr(b3), Bits.HighestOrderBit(b3));
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
            ErrorPacket p = new ErrorPacket(4, "Some stuff went wrong.");
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

        static void TwoBitTest()
        {
            // Represents 0b01110010 (114)
            TwoBit b0 = new TwoBit(1, 0);
            TwoBit b1 = new TwoBit(0, 0);
            TwoBit b2 = new TwoBit(1, 1);
            TwoBit b3 = new TwoBit(0, 1);
            Console.WriteLine(Bits.TwoBitsToByte(new TwoBit[] { b0, b1, b2, b3 }));

            Console.WriteLine(Bits.TwoBitsToByte(new TwoBit[] { b0, b0 }));
        }

        static void ByteToTwoBitsTest()
        {
            byte b = 1;
            Console.WriteLine(Bits.ByteToTwoBits(b).Length);
            Console.WriteLine(Bits.ByteToTwoBits(b, 3).Length);

            b = 0xF0;
            Console.WriteLine(Bits.ByteToBinStr(b));
            Console.WriteLine(Bits.ByteToTwoBits(b).Length);
        }

        static void InvertBitsTest()
        {
            byte b = 0xC4;
            Console.WriteLine(Bits.ByteToBinStr(b));
            Console.WriteLine(Bits.ByteToBinStr(Bits.InvertBits(b)));
        }

        static void ThereAndBack()
        {
            var tb = Bits.ByteToTwoBits(0x2, 2);
            Console.WriteLine("{0},{1}", tb[0].AsByte(), tb[1].AsByte());
            TwoBit b0 = new TwoBit(0, 0);
            TwoBit b2 = new TwoBit(1, 0);
            byte res = Bits.TwoBitsToByte(new TwoBit[] { b2, b0 });
            Console.WriteLine(res);
        }

        static void HasEvenOnesTest()
        {
            uint i = 13422208 - 2;
            Console.WriteLine(Bits.Int32ToBinStr(i));
            Console.WriteLine(Bits.HasEvenOnes(i));
        }

        static void FlipNthBitTest()
        {
            uint i = 13422208;
            Console.WriteLine(Bits.Int32ToBinStr(i));
            Console.WriteLine(Bits.Int32ToBinStr(Bits.FlipNthBit(i, 5)));
        }

        static void HammingCodeTest()
        {
            // 0b00010100011010000010011101
            // uint i = 5349533;
            // 0b10010100011010000010011101
            uint i = 38903965;
            HammingCodePrint(Hamming32Bit.HammingCode1, i);
            HammingCodePrint(Hamming32Bit.HammingCode2, i);
            HammingCodePrint(Hamming32Bit.HammingCode4, i);
            HammingCodePrint(Hamming32Bit.HammingCode8, i);
            HammingCodePrint(Hamming32Bit.HammingCode16, i);
            HammingCodePrint(Hamming32Bit.HammingCode32, i);
        }

        static void HammingCodePrint(Func<uint, uint> codeFunc, uint val)
        {
            uint hamming = codeFunc(val);
            Console.WriteLine(Bits.Int32ToBinStr(hamming));
        }

        public static void PrintByteArr(byte[] bytes)
        {
            foreach (byte b in bytes)
            {
                Console.Write("{0} ", Bits.ByteToBinStr(b));
            }
            Console.WriteLine();
        }

        static readonly string USAGE_MSG = "Usage : [mono] HammingTFTP.exe [ error | noerror ] tftp−host file";

        public static void Main(string[] args)
        {

            if (args.Length != 3)
            {
                Console.WriteLine(USAGE_MSG);
                Console.ReadKey();
                return;
            }

            RequestType reqType = (RequestType) Enum.Parse(typeof(RequestType), args[0], ignoreCase:true);
            string hostName = args[1];
            string fileName = args[2];

            HammingCodeTest();

            //HammingClient client = new HammingClient(reqType, hostName, fileName);
            //client.Retrieve();
            
            
            // wait to end
            Console.ReadKey(true);
        }
    }
}
