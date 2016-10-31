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

            using (HammingClient client = new HammingClient(reqType, hostName, fileName))
            {
                try
                {
                    if (client.Connected)
                    {
                        // Transfer and write the given file from the server
                        client.RetrieveAndWrite();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("The client encountered an error: {0}", e.Message);
                    Console.WriteLine("The client will now close.");
                }
            }
        }
    }
}
