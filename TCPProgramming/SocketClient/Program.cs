using System;
using System.Text;
using System.Net.Sockets;
using System.IO;

namespace SocketClient {
    class Program {
        static void Main(string[] args) {
            try {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer 
                // connected to the same address as specified by the server, port
                // combination.
                int port = 9000;
                string filename, ip;
                
                if (args.Length < 2) {
                    Console.WriteLine("You should open the program like ./SocketClient.exe <ip> <filename>");
                    return;
                }

                ip = args[0];
                filename = args[1];
                TcpClient client = new TcpClient(ip, port);

                // Translate the passed message into ASCII and store it as a Byte array.
                byte[] data = System.Text.Encoding.ASCII.GetBytes(filename);

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();
                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                // Receive the TcpServer.response.
                // Buffer to store the response bytes.
                data = new byte[1000];

                // Check if file was found server side
                stream.Read(data, 0, 1);

                if (Encoding.ASCII.GetString(data).StartsWith("0"))
                {
                    Console.WriteLine("Server replied that it couldn't find the file!");
                    stream.Close();
                    client.Close();
                    return;
                }
                else
                {
                    Console.WriteLine("Server found the file and is now sending! MSG: {0}", Encoding.ASCII.GetString(data));
                }

                // Remove path from filename.
                string file = Path.GetFileName(filename);
                // Create BinaryWriter to write the read data to a file.
                var writer = new BinaryWriter(File.Open(file, FileMode.Create));
                int i, counter = 1;
                do
                {
                    i = stream.Read(data, 0, data.Length);
                    writer.Write(data, 0, i);

                    Console.WriteLine("#{0} - {1} bytes", counter, i);

                    counter++;
                } while (i > 0);

                writer.Close();
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e) {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e) {
                Console.WriteLine("SocketException: {0}", e);
            }

            Console.WriteLine("\n Press Enter to continue...");
            Console.Read();
        }
    }
}
