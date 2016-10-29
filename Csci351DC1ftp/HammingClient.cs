using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

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


            byte[] bytes = datagram.GetBytes();
            Program.PrintByteArr(bytes);
            // byte[] bytes = (new AckPacket(0)).GetBytes();
            this._con.Send(bytes, bytes.Length);

            byte[] resp = this._con.Receive(ref this._remoteEP);
            short respCode = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(resp, 0));
            Console.WriteLine("The datagram received was type: {0}", (Opcode)respCode);
        }

        public void Dispose()
        {
            this._con.Close();
        }
    }
}
