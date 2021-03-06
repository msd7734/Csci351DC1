﻿/*
 * By: Matthew Dennis (msd7734)
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Csci351DC1ftp
{
    /// <summary>
    /// And outgoing packet to request a file.
    /// </summary>
    public class RequestPacket : TFTPPacket
    {
        // Only need to worry about one Transfer Mode apparently
        private static byte[] TRANSFER_MODE_BYTES = Encoding.ASCII.GetBytes("octet");


        public string FileName { get; private set; }

        public RequestPacket(string fileName)
            : base(Opcode.RRQ)
        {
            FileName = fileName;
        }

        public override byte[] GetBytes()
        {
            byte[] opcBytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)this.Opcode));
            byte[] fnameBytes = Encoding.ASCII.GetBytes(FileName);
            byte[] nul = { 0x0 };

            return opcBytes.Concat(fnameBytes)
                .Concat(nul)
                .Concat(TRANSFER_MODE_BYTES)
                .Concat(nul)
                .ToArray();
        }
    }
}
