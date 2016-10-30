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

        // The max number of data bytes expected in a data packet
        private static readonly int MAX_DATA_SIZE = 512;
        // ...which are Hamming encoded in 32-bit (4 byte) blocks
        private static int BLK_SIZE = 4;
        // Indeces of Hamming parity bits
        private static byte[] HAMMING_BIT_INDECES = { 0, 1, 3, 7, 15, 31 };

        private static readonly Dictionary<string, string> SPECIAL_HOSTS =
            new Dictionary<string, string>()
            {
                {"kayrun", "kayrun.cs.rit.edu"},
                {"localhost", "127.0.0.1."}
            };

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

        public void Retrieve()
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

            using (FileStream file = new FileStream(this._fileName, FileMode.Create))
            {
                byte[] trueData;
                do
                {
                    // There's potentially a really annoying edge case here where there are trailing null bytes
                    // but the next packet has valid data. So null-byte padding that just happens to fall on a packet boundary:
                    //      [..., 0x0, 0x0, 0x0, 0x0] | [0xF0, 0xD3, 0x18, ...]
                    // The way to fix this might be to write two packets at a time?
                    // But if you're saving one and checking on the next one, it's possible there will be no next packet (timeout).
                    // Either need to add filesize or total expected incoming blocks to the protocol, or just assume this edge case won't happen.
                    // Since we can't change the protocol, assume it won't happen.

                    trueData = UnhamData(mostRecentData.GetTruncatedData());

                    // file.Write(trueData, 0, trueData.Length);
                    resp = ReceiveDatagram(new AckPacket(mostRecentData.BlockNum));

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

                } while (trueData.Length == MAX_DATA_SIZE);
                
            }

            // not handling error correction as of yet
        }

        private byte[] UnhamData(byte[] data)
        {
            //mmm, ham...

            // 1. Invert byte order so that the hamming bits can be properly extracted/read
            // 2. Save extra trailing bits
            // 3. Append the trailing bits from the previous step (if any) as LEADING bits
            // 4. The trailing bits are now the trailing produced from the append, followed by the trailing from this step
            // - Trailing from previous: 00
            // - This step: 10101000 01011010 100110[01 00]
            // - Prepend previous trailing: 00101010 00010110 10100110 [0100]
            // 5. Invert ALL BITS so that they properly translate into the data
            // 6. Read the next four bytes and repeat at 1. until data is consumed

            int numBlocks = data.Length / BLK_SIZE;
            byte[] block = new byte[BLK_SIZE];
            
            for (int i = 0; i < numBlocks; i += BLK_SIZE)
            {
                Array.Copy(data, i, block, 0, BLK_SIZE);
                Array.Reverse(block);
                uint bits = BitConverter.ToUInt32(block, 0);
                bits = Bits.SnipBits(bits, HAMMING_BIT_INDECES);
                block = BitConverter.GetBytes(bits);


                Array.Clear(block, 0, block.Length);
            }

            return data;
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
                throw new Exception("Failed to get a response from the server (likely due to a timeout).", se);
            }

            short respCode = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(resp, 0));

            if ((Opcode)respCode == Opcode.DATA)
            {
                short blockNum = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(resp, 2));
                byte[] data = new byte[MAX_DATA_SIZE];
                for (int i = 0; i < data.Length; ++i)
                {
                    data[i] = resp[i + 4];
                }
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
