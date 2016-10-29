using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Csci351DC1ftp
{
    public class ErrorPacket : TFTPPacket
    {
        public short ErrorNum { get; private set; }

        public string ErrorMsg { get; private set; }

        public ErrorPacket(short errNum, string errMsg)
            : base(Opcode.ERROR)
        {
            ErrorNum = errNum;
            ErrorMsg = errMsg;
        }

        public override byte[] GetBytes()
        {
            // remember converting str => bytes does NOT include null terminator

            byte[] opcBytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)this.Opcode));
            byte[] errnBytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(ErrorNum));
            byte[] errmsgBytes = Encoding.ASCII.GetBytes(ErrorMsg);
            byte[] nul = { 0x0 };

            return opcBytes.Concat(errnBytes)
                .Concat(errmsgBytes)
                .Concat(nul).ToArray();
        }
    }
}
