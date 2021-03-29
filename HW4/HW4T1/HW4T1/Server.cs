using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace HW4T1
{
    /// <summary>
    /// Server.
    /// </summary>
    public class Server
    {
        private TcpListener listener;
        
        public Server(string host, int port)
        {
            this.listener = new TcpListener(IPAddress.Parse(host), port);
            listener.Start();
        }

        /// <summary>
        /// Start the server.
        /// </summary>
        public async Task Process()
        {
            while (true)
            {
                var client = await listener.AcceptTcpClientAsync();
                await Task.Run(() => this.Run(client));
            }
        }

        /// <summary>
        /// Run the server.
        /// </summary>
        private async Task Run(TcpClient client)
        {
            using var stream = client.GetStream();

            var reader = new StreamReader(stream);
            var writer = new StreamWriter(stream) { AutoFlush = true };
            var data = await reader.ReadLineAsync();
            var (command, path) = this.GetCommandAndPath(data);

            switch (command)
            {
                case "1":
                    await this.List(path, writer);
                    break;

                case "2":
                    await this.Get(path, writer);
                    break;

                default:
                    throw new ArgumentException("Incorrect request!");
            }
        }

        /// <summary>
        /// Listing files and folders from the path.
        /// </summary>
        private async Task List(string path, StreamWriter writer)
        {
            if (!Directory.Exists(path))
            {
                await writer.WriteLineAsync("-1");
                return;
            }

            var files = Directory.GetFiles(path);
            var directories = Directory.GetDirectories(path);
            
            var responce = (directories.Length + files.Length).ToString();

            foreach (var file in files)
            {
                var filePath = file.Remove(0, file.IndexOf(path));
                responce += $" {filePath} false";
            }

            foreach (var directory in directories)
            {
                var directoryPath = directory.Remove(0, directory.IndexOf(path));
                responce += $" {directoryPath} true";
            }

            await writer.WriteLineAsync(responce);
        }

        /// <summary>
        /// Get file from the path. 
        /// </summary>
        private async Task Get(string path, StreamWriter writer)
        {
            if (!File.Exists(path))
            {
                await writer.WriteLineAsync("-1");
                return;
            }
            await writer.WriteAsync($"{new FileInfo(path).Length} ");
            using var fileStream = File.OpenRead(path);
            await fileStream.CopyToAsync(writer.BaseStream);
            await writer.WriteLineAsync();
        }

        /// <summary>
        /// Parse request.
        /// </summary>
        private (string, string) GetCommandAndPath(string data) 
            => (data.Split(' ')[0], data.Split(' ')[1]);
    }
}
