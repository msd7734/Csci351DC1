using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csci351DC1ftp
{
    public enum CodewordError
    {
        None,
        OneBit,
        TwoBit
    }

    public class Codeword
    {
        public CodewordError ErrorType { get; private set; }

        public uint Value { get; private set; }

        public Codeword(uint hammingCode, byte parityBit)
        {

        }

        private void CheckError(uint hammingCode, byte parityBit)
        {
            
        }
    }
}
