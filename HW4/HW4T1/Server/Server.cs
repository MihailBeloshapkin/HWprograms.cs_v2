using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    // This class contains methods for ftp server.
    public class Server
    {
        private readonly TcpListener listener;

        public Server(IPAddress localAddress, int port)
        {
            this.listener = new TcpListener(localAddress, port);
            this.listener.Start();
        }
    }
}
