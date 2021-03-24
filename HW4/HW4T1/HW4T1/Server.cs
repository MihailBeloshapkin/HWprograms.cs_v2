using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace HW4T1
{
    public class Server
    {
        private readonly TcpListener listener;

        public Server(string host, int port)
        {
            this.listener = new TcpListener(IPAddress.Parse(host), port);
            
        }

        public async Task Run()
        {
            this.listener.Start();

            while (true)
            {
                var client = await listener.AcceptTcpClientAsync();
                await Task.Run(() => this.ProcessRequests(client));
            }
        }

        private async Task ProcessRequests(TcpClient client)
        {
            using var stream = client.GetStream();
            var reader = new StreamReader(stream);
            var writer = new StreamWriter(stream) { AutoFlush = true };
            var request = await reader.ReadLineAsync();
            var (command, path) = this.ParseData(request);

            switch (command)
            {
                case "1":
                    await this.List(path, writer);
                    break;
                case "2":
                    await this.Get(path, writer);
                    break;
            }
        }

        private (string, string) ParseData(string request) => (request.Split()[0], request.Split()[1]);

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
