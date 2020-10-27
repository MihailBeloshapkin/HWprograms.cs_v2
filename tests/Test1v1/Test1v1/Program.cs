using System;
using System.Threading;
using System.Xml.Schema;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace Test1v1
{
    class Program
    {
        static Server server;
        static TcpClient client;
        static NetworkStream stream;

        static void Main(string[] args)
        {
            int port = 0;
            string ip;
            
            if (args.Length == 1)
            {
                port = int.Parse(args[0]);
                try
                {
                    server = new Server();
                    var ListenThread = new Thread(server.Listen);
                    ListenThread.Start();
                }
                catch (Exception)
                {
                    server.Disconnect();
                }
            }
            else 
            {
                port = int.Parse(args[0]);
                ip = args[1];
                try
                {
                    client = new TcpClient();
                    client.Connect(ip, port);
                    stream = client.GetStream();
                }
                catch (Exception)
                {

                }
            }
        }
    }
}
