using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
            ProtocolType.Udp);

        IPAddress broadcast = IPAddress.Parse("188.166.29.138");

        byte[] sendbuf = Encoding.ASCII.GetBytes("u");
        IPEndPoint ep = new IPEndPoint(broadcast, 12345);

        s.SendTo(sendbuf, ep);

        Console.WriteLine("Message sent to the broadcast address");

        sendbuf = new byte[100];
        s.Receive(sendbuf);

        Console.WriteLine("Received: {0}", Encoding.ASCII.GetString(sendbuf));
    }
}