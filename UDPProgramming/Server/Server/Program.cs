using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            bool done = false;

            const int listenPort = 12345;
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);

            UdpClient listener = new UdpClient(listenPort);

            try
            {
                while (!done)
                {
                    Console.WriteLine("Waiting for broadcast");
                    byte[] bytes = listener.Receive(ref groupEP);

                    string data = Encoding.ASCII.GetString(bytes, 0, bytes.Length);

                    Console.WriteLine("Received broadcast from {0}:\n{1}\n",
                        groupEP.ToString(),
                        data);

                    string reply = "Wrong command.";

                    if (data.ToLower() == "u")
                    {
                        // Svar med uptime
                        reply = ReadUptime();
                    }
                    else if (data.ToLower() == "l")
                    {
                        // Svar med loadavg
                        reply = ReadLoadavg();
                    }

                    bytes = Encoding.ASCII.GetBytes(uptime);
                    listener.Send(bytes, bytes.Length, groupEP);

                    Console.WriteLine("Replied: {0}", uptime);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static string ReadUptime()
        {
            if (File.Exists("/proc/uptime"))
            {
                string line = File.ReadLines("/proc/uptime").First();
                string[] times = line.Split(' ');

                return times[0] + " seconds";
            }

            return "File not found";
        }
    }
}
