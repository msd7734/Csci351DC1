﻿using System;
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

            /*
            if (args.Length != 3)
            {
                Console.WriteLine(USAGE_MSG);
                Console.ReadKey();
                return;
            }

            RequestType reqType = (RequestType) Enum.Parse(typeof(RequestType), args[0], ignoreCase:true);
            string hostName = args[1];
            string fileName = args[2];

            HammingClient client = new HammingClient(reqType, hostName, fileName);
            client.Retrieve();
            */

            HighestOrderBitTest();

            // wait to end
            Console.ReadKey(true);
        }
    }
}
