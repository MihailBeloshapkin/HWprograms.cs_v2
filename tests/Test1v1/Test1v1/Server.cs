using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Data;

namespace Test1v1
{
    public class Server
    {
        private TcpListener tcpListener;
        private Client client;

        public void Connect(Client client)
        {
            this.client = client;
        }

        public void Disconnect()
        {
            this.tcpListener.Stop();
        }

        public void Listen()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Any, 8888);
                tcpListener.Start();
                Console.WriteLine("This server has benn just started");

                while (true)
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();
                    Client clientObject = new Client(tcpClient, this);
                    Thread clientThread = new Thread(new ThreadStart(client.Process));
                    clientThread.Start();
                }
            }
            catch (Exception)
            {
                this.Disconnect();
            }
        }

        public void SendMessage(string message)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            this.client.Stream.Write(data, 0, data.Length);
        }
    }
}
