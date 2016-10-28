using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

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

    public class HammingClient
    {
        // Server always listens on port 7000
        private static readonly int PORT = 7000;

        private static readonly Dictionary<string, string> SPECIAL_HOSTS =
            new Dictionary<string, string>()
            {
                {"kayrun", "kayrun.cs.edu"},
                {"localhost", "localhost"} // re-check how to connect on localhost!
            };

        private RequestType _reqType;
        private TcpClient _con;
        private string _fileName;

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
            this._con = new TcpClient();
            this._fileName = fileName;

            try
            {
                this._con.Connect(ConvertSpecialHostName(remote), PORT);
            }
            catch (SocketException se)
            {
                // allow connection to fail, IsConnected method will report not connected
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to connect to server: {0}", e.Message);
            }
        }

        public void Retrieve()
        {
            if (!IsConnected())
            {
                throw new Exception(
                    String.Format("Remote connection is not initialized, so {0} cannot be retrieved.", this._fileName)
                );
            }

            
        }

        /// <summary>
        /// Whether this client has successfully connected to a remote host.
        /// </summary>
        /// <returns>True if connected, false otherwise.</returns>
        public bool IsConnected()
        {
            return _con.Connected;
        }
    }
}
