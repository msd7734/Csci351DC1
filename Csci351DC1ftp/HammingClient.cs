using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace Csci351DC1ftp
{
    /// <summary>
    /// Define whether a HammingClient should make requests for data with errors.
    /// </summary>
    public enum RequestType
    {
        NoError,
        Error
    }

    public class HammingClient : IDisposable
    {
        // Server always listens on port 7000
        private static readonly int PORT = 7000;

        // Timeout after 10 seconds
        private static readonly int TIMEOUT = 1000 * 12;

        // The max number of data bytes expected in a data packet...
        private static readonly int MAX_DATA_SIZE = 512;

        // ...which are Hamming encoded in 32-bit (4 byte) blocks
        private static int BLK_SIZE = 4;

        // The max data per packet after removing hamming bits
        private static int MAX_EXTRACT_DATA_SIZE = MaxExtractedDataSize();

        // Indeces of Hamming parity bits
        private static byte[] HAMMING_BIT_INDECES = { 0, 1, 3, 7, 15, 31 };

        private static readonly byte ERR_BYTE = 0xFF;

        private static readonly Dictionary<string, string> SPECIAL_HOSTS =
            new Dictionary<string, string>()
            {
                {"kayrun", "kayrun.cs.rit.edu"},
                {"localhost", "127.0.0.1."}
            };

        private static int MaxExtractedDataSize()
        {
            // Yes, we know 32-bit hamming encoded blocks means 26 bits of data.
            // But how to arrive at that number? Like this.
            int blockBits = BLK_SIZE * 8;
            int hammingBits = (int)Math.Log(blockBits, 2) + 1;
            double byteYieldPerDecode = (blockBits - hammingBits) / 8.0;
            int extractedDataSize = (int)((MAX_DATA_SIZE / BLK_SIZE) * byteYieldPerDecode);
            return extractedDataSize;
        }

        private RequestType _reqType;
        private UdpClient _con;
        private string _fileName;
        private IPEndPoint _remoteEP;

        /// <summary>
        /// Whether this client has successfully connected to a remote host.
        /// </summary>
        /// <returns>True if connected, false otherwise.</returns>
        public bool Connected { get; private set; }

        /// <summary>
        /// Convert a hostname to a special remote known to the HammingClient, if possible.
        /// </summary>
        /// <param name="host">The host name to convert.</param>
        /// <returns>
        /// The equivalent host name used to connect to the remote if it's special,
        /// otherwise the same host name passed in.
        /// </returns>
        private string ConvertSpecialHostName(string host)
        {
            if (SPECIAL_HOSTS.ContainsKey(host.ToLower()))
            {
                return SPECIAL_HOSTS[host];
            }
            else
            {
                return host;
            }
        }

        /// <summary>
        /// Construct a new HammingClient and initialize the connection if possible.
        /// </summary>
        /// <param name="reqType">The type of requests to make.</param>
        /// <param name="remote">The remote host name.</param>
        /// <param name="fileName">The name of the file to retrieve from the server.</param>
        public HammingClient(RequestType reqType, string remote, string fileName)
        {
            this._reqType = reqType;
            this._con = new UdpClient();
            this._con.Client.ReceiveTimeout = TIMEOUT;
            this._fileName = fileName;

            try
            {
                string parsedRemote = ConvertSpecialHostName(remote);
                this._con.Connect(parsedRemote, PORT);
                this._remoteEP = (IPEndPoint)this._con.Client.RemoteEndPoint;
                Connected = true;
            }
            catch (SocketException se)
            {
                Console.WriteLine(se.Message);
                Connected = false;
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to connect to server: {0}", e.Message);
                Connected = false;
            }
        }

        public void RetrieveAndWrite()
        {
            if (!Connected)
            {
                throw new Exception(
                    String.Format("Remote connection is not initialized, so {0} cannot be retrieved.", this._fileName)
                );
            }

            TFTPPacket datagram;
            if (this._reqType == RequestType.Error)
            {
                datagram = new RequestCorruptedPacket(this._fileName);
            }
            else
            {
                // implicitly with no error
                datagram = new RequestPacket(this._fileName);
            }

            TFTPPacket response = ReceiveDatagram(datagram);

            if (response.Opcode == Opcode.DATA)
            {
                HandleData((DataPacket)response);
            }
            else
            {
                // implicitly an error packet
                ReportError((ErrorPacket)response);
            }
        }

        private void HandleData(DataPacket firstPacket)
        {
            DataPacket mostRecentData = firstPacket;
            TFTPPacket resp;
            string tempFile = this._fileName + ".tmp";

            using (FileStream file = new FileStream(tempFile, FileMode.Create))
            {
                byte[] trueData;
                bool isLastPacket = false;
                do
                {
                    isLastPacket = mostRecentData.Data.Length != MAX_DATA_SIZE;
                    trueData = UnhamData(mostRecentData.Data, isLastPacket);

                    if (trueData.Length == 1 && trueData[0] == ERR_BYTE)
                    {
                        Console.WriteLine("2-bit Error in block {0}. Sending NACK.", mostRecentData.BlockNum);
                        resp = ReceiveDatagram(new NackPacket(mostRecentData.BlockNum));
                    }
                    else
                    {
                        file.Write(trueData, 0, trueData.Length);
                        Console.WriteLine("Wrote block {0} ({1} bytes)", mostRecentData.BlockNum, trueData.Length);
                        resp = ReceiveDatagram(new AckPacket(mostRecentData.BlockNum));
                    }

                    if (resp.Opcode == Opcode.DATA)
                    {
                        mostRecentData = (DataPacket)resp;
                    }
                    else
                    {
                        // implicitly an error packet
                        ReportError((ErrorPacket)resp);
                        return;
                    }

                } while (!isLastPacket);
            }

            using (FileStream tmp = new FileStream(tempFile, FileMode.Open))
            {
                using (FileStream dest = new FileStream(this._fileName, FileMode.Create))
                {
                    tmp.CopyTo(dest);
                }
            }

            File.Delete(tempFile);
        }

        private byte[] UnhamData(byte[] data, bool lastPacket)
        {
            //mmm, ham...

            int numBlocks = data.Length / BLK_SIZE;
            byte[] block = new byte[BLK_SIZE];
            List<TwoBit> extraBits = new List<TwoBit>(4);

            List<byte> unhammed = new List<byte>(MAX_EXTRACT_DATA_SIZE);
            bool lastBlock = false;
            for (int i = 0; i < data.Length; i += BLK_SIZE)
            {
                lastBlock = ((i + BLK_SIZE) / BLK_SIZE >= numBlocks) && lastPacket;

                if (i + BLK_SIZE > data.Length)
                {
                    Array.Copy(data, i, block, 0, data.Length - i);
                }
                else
                {
                    Array.Copy(data, i, block, 0, BLK_SIZE);
                }

                if (!BitConverter.IsLittleEndian)
                    Array.Reverse(block);

                // uint bits is the binary representation of the block
                //  with high order bits to the left, low order to the right
                uint bits = BitConverter.ToUInt32(block, 0);

                // for every hamming codeword n that is not even when 
                //  checked against its parity bit, add 2^n to error bit index.
                //  flip error bit index, and check the 32nd bit (overall parity)
                //  if it's still not even parity, RUN FOR THE HILLS (or just send NACK)

                // check and correct 1-bit errors
                bits = CheckBitErrors(bits);
                // bit 32 in uint bits is the parity bit, so check parity one last time
                //  if it's still not even, return error byte
                if (!Bits.HasEvenOnes(bits))
                {
                    return new byte[] { ERR_BYTE };
                }

                // remove hamming bits
                bits = Bits.SnipBits(bits, HAMMING_BIT_INDECES); ;

                // make room to prepend previous leftover bits
                bits = bits << (HAMMING_BIT_INDECES.Length - (2 * extraBits.Count));

                // convert the previous leftover bits to a byte
                byte extraByte = Bits.TwoBitsToByte(extraBits.ToArray());
                extraByte = (byte)(Bits.InvertBits(extraByte) >> (8 - (2 * extraBits.Count)));

                // move the bits to the head of a 32-bit mask
                uint prependMask = (uint)(extraByte << (32 - (2 * extraBits.Count)));

                // XOR the shifted, unhammed bits with the mask
                bits = bits ^ prependMask;

                block = BitConverter.GetBytes(bits);

                block[0] = Bits.InvertBits(block[0]);
                block[1] = Bits.InvertBits(block[1]);
                block[2] = Bits.InvertBits(block[2]);
                block[3] = Bits.InvertBits(block[3]);

                byte trailingByte = (BitConverter.IsLittleEndian) ? block[0] : block[3];


                // store the new trailing bits (if previously 1 trailing pair, take 2 this time, and so on)
                TwoBit[] newTrailing = Bits.ByteToTwoBits(trailingByte, extraBits.Count + 1);
                extraBits.Clear();
                extraBits.AddRange(newTrailing);

                if (BitConverter.IsLittleEndian)
                {
                    if (lastBlock)
                    {
                        block = TruncateBlock(block);
                        for (int b = block.Length - 1; b >= 0; --b)
                            unhammed.Add(block[b]);
                    }

                    else
                    {
                        unhammed.Add(block[3]);
                        unhammed.Add(block[2]);
                        unhammed.Add(block[1]);

                        // if we can make a full byte from the trailing bits, we can write it
                        if (extraBits.Count == 4)
                        {
                            byte leftover = Bits.TwoBitsToByte(extraBits.ToArray());
                            unhammed.Add(leftover);
                            extraBits.Clear();
                        }
                    }
                }
                else
                {
                    if (lastBlock)
                    {
                        block = TruncateBlock(block);
                        for (int b = 0; b < block.Length; ++b)
                            unhammed.Add(block[b]);
                    }
                    else
                    {
                        // if we can make a full byte from the trailing bits, we can write it
                        if (extraBits.Count == 4)
                        {
                            byte leftover = Bits.TwoBitsToByte(extraBits.ToArray());
                            unhammed.Add(leftover);
                            extraBits.Clear();
                        }

                        unhammed.Add(block[1]);
                        unhammed.Add(block[2]);
                        unhammed.Add(block[3]);
                    }
                }

                Array.Clear(block, 0, block.Length);
            }

            return unhammed.ToArray();
        }

        private uint CheckBitErrors(uint binSeq)
        {
            sbyte bitToCorrect = 0;

            uint hcode1 = Hamming32Bit.HammingCode1(binSeq);
            if (!Bits.HasEvenOnes(hcode1))
                bitToCorrect += 1;

            uint hcode2 = Hamming32Bit.HammingCode2(binSeq);
            uint bit2 = Bits.NthBit(binSeq, 1);
            if (!Bits.HasEvenOnes(hcode2))
                bitToCorrect += 2;

            uint hcode4 = Hamming32Bit.HammingCode4(binSeq);
            uint bit4 = Bits.NthBit(binSeq, 3);
            if (!Bits.HasEvenOnes(hcode4))
                bitToCorrect += 4;

            uint hcode8 = Hamming32Bit.HammingCode8(binSeq);
            uint bit8 = Bits.NthBit(binSeq, 7);
            if (!Bits.HasEvenOnes(hcode8))
                bitToCorrect += 8;

            uint hcode16 = Hamming32Bit.HammingCode16(binSeq);
            uint bit16 = Bits.NthBit(binSeq, 15);
            if (!Bits.HasEvenOnes(hcode16))
                bitToCorrect += 16;

            bitToCorrect -= 1;

            // return the (ostensibly) corrected stream

            if (bitToCorrect > -1)
            {
                return Bits.FlipNthBit(binSeq, (byte)bitToCorrect);
            }
                
            
            return binSeq;
        }

        private byte[] TruncateBlock(byte[] b)
        {
            // starting index of null bytes
            int nullStartIndex = -1;

            if (BitConverter.IsLittleEndian)
                Array.Reverse(b);

            for (int i = 0; i < b.Length; ++i)
            {
                if (b[i] == 0x0 && nullStartIndex == -1)
                {
                    nullStartIndex = i;
                }
                else if (b[i] != 0x0)
                {
                    nullStartIndex = -1;
                }
            }

            byte[] res = (nullStartIndex >= 0) ?
                b.Take(nullStartIndex).ToArray() :
                b;

            if (BitConverter.IsLittleEndian)
                Array.Reverse(res);

            return res;
        }

        private TFTPPacket ReceiveDatagram(TFTPPacket toSend)
        {
            byte[] bytes = toSend.GetBytes();
            this._con.Send(bytes, bytes.Length);

            byte[] resp;
            try
            {
                resp = this._con.Receive(ref this._remoteEP);
            }
            catch (SocketException se)
            {
                throw new Exception("Failed to get a response from the server.", se);
            }

            short respCode = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(resp, 0));

            if ((Opcode)respCode == Opcode.DATA)
            {
                short blockNum = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(resp, 2));
                byte[] data = new byte[resp.Length - 4];
                Console.WriteLine("Got incoming data packet. Size: {0}", resp.Length - 4);
                Array.Copy(resp, 4, data, 0, resp.Length - 4);
                return new DataPacket(blockNum, data);
            }
            else if ((Opcode)respCode == Opcode.ERROR)
            {
                short errNum = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(resp, 2));
                string msg = BitConverter.ToString(resp, 4);
                return new ErrorPacket(errNum, msg);
            }
            else
            {
                throw new Exception(String.Format("Unexpected response from server: code {0}", respCode));
            }
        }

        private void ReportError(ErrorPacket err)
        {
            Console.WriteLine("The server returned an error: {0} {1}", err.ErrorNum, err.ErrorMsg);
        }

        public void Dispose()
        {
            this._con.Close();
        }
    }
}
