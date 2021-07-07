using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace Test1v1
{
    public class Client
    {
        private TcpClient client;
        private Server server;
        protected internal NetworkStream Stream { get; private set; }

        public Client(TcpClient tcpClient, Server server)
        {
            this.client = tcpClient;
            this.server = server;
            server.Connect(this);
        }

        public void Process()
        {
            try
            {
                Stream = client.GetStream();
                string message;

                while (true)
                {
                    message = GetMessage();
                    Console.WriteLine(message);
                    server.SendMessage(message);
                }
            }
            catch (Exception)
            {
                server.Disconnect();
                Stream.Close();
                client.Close();
            }
        }

        private string GetMessage()
        {
            byte[] data = new byte[64];
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            while (Stream.DataAvailable)
            {
                bytes = Stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }

            return builder.ToString();
        }

    }
}

