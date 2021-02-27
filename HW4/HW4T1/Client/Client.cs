using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Net.Sockets;


namespace Client
{

    // THis class contains ftp client methods.
    class Client
    {
        private readonly StreamReader reader;
        private readonly StreamWriter writer;
        private readonly TcpClient tcpClient;
        private readonly Stream stream;

        public Client(int port, string hostName)
        {
            this.tcpClient = new TcpClient(hostName, port);
            this.stream = tcpClient.GetStream();
            this.writer = new StreamWriter(stream) { AutoFlush = true };
            this.reader = new StreamReader(stream);
        }

    }
}
