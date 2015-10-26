using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace SocketServer {
    class Program {
        public static void Main() {
            TcpListener server = null;
            try {
                // Set the TcpListener on port 13000.
                Int32 port = 9000;
                IPAddress localAddr = IPAddress.Parse("0.0.0.0");

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;

                // Enter the listening loop.
                Console.Write("Waiting for a connection... ");

                // Perform a blocking call to accept requests.
                // You could also user server.AcceptSocket() here.
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Connected!");

                data = null;

                // Get a stream object for reading and writing
                NetworkStream stream = client.GetStream();

                int i = stream.Read(bytes, 0, bytes.Length);

                // Translate data bytes to a ASCII string.
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                Console.WriteLine("Received: {0}", data);

                // Check if file exists
                if (File.Exists(data)) {
                    var reader = new BinaryReader(File.Open(data, FileMode.Open));

                    int counter = 1;
                    byte[] array;

                    array = Encoding.ASCII.GetBytes("1");
                    stream.Write(array, 0, array.Length);

                    do
                    {
                        array = reader.ReadBytes(1000);
                        stream.Write(array, 0, array.Length);

                        Console.WriteLine("#{0} - {1} bytes", counter, array.Length);

                        counter++;
                    } while (array.Length > 0);

                    reader.Close();
                }
                else
                {
                    byte[] toSend = Encoding.ASCII.GetBytes("0");
                    stream.Write(toSend, 0, toSend.Length);
                }

                // Shutdown and end connection
                stream.Close();
                client.Close();
            }
            catch (SocketException e) {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally {
                // Stop listening for new clients.
                server.Stop();
            }
        }
    }
}
