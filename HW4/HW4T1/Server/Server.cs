using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

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

        public async Task Run()
        {
            while (true)
            {
                var socket = await listener.AcceptSocketAsync();
                
            }
        }

        /// <summary>
        /// Listing
        /// </summary>
        private async Task List(string path, StreamWriter writer)
        {
            if (!Directory.Exists(path))
            {
                await writer.WriteLineAsync("-1");
                return;
            }

            var listOfDirictories = Directory.GetDirectories(path);
            var setOfFiles = Directory.GetFiles(path);

            var response = (listOfDirictories.Length + setOfFiles.Length).ToString();

            foreach (var direcotry in listOfDirictories)
            {
                response += $"{direcotry} -> directory";
            }

            foreach (var file in setOfFiles)
            {
                response += $"{file} -> file";
            }

            await writer.WriteAsync(response);
        }

        private async Task Get(string path, StreamWriter writer)
        {
            if (!File.Exists(path))
            {
                await writer.WriteLineAsync("-1");
                return;
            }

            await writer.WriteAsync($"{new FileInfo(path).Length}");
            using var fileStream = File.OpenRead(path);
            await fileStream.CopyToAsync(writer.BaseStream);
            await writer.WriteLineAsync();
        }

    }
}
