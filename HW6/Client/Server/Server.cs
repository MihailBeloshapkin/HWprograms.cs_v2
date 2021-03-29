using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace HW6T1
{
    /// <summary>
    /// FTP server.
    /// </summary>
    public class Server
    {
        //    private int port;
        private TcpListener listener;

        /// <summary>
        /// Server constructor
        /// </summary>
        /// <param name="host">host name</param>
        /// <param name="port">port name</param>
        public Server(string host, int port)
        {
            //    this.port = port;
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
        /// Stop the server.
        /// </summary>
        public void ShutDown() => listener.Stop();

        /// <summary>
        /// Server process
        /// </summary>
        /// <param name="client">Tcp client</param>
        private async Task Run(TcpClient client)
        {
            using var stream = client.GetStream();

            var reader = new StreamReader(stream);
            var writer = new StreamWriter(stream) { AutoFlush = true };
            var data = await reader.ReadLineAsync();
            var (command, path) = ParseData(data);

            switch (command)
            {
                case "1":
                    await this.List(path, writer);
                    break;

                case "2":
                    await this.Get(path, writer);
                    break;

                default:
                    throw new ArgumentException("Command error.");
            }
        }

        /// <summary>
        /// Data parsing.
        /// </summary>
        /// <param name="data">String with command and path</param>
        /// <returns>Command and path</returns>
        private (string, string) ParseData(string data) => (data.Split()[0], data.Split()[1]);

        /// <summary>
        /// Comand list.
        /// </summary>
        /// <param name="path">Path to the file</param>
        /// <param name="writer">Stream writer</param>
        /// <returns><size: Int> (<name: String> <isDir: Boolean>)</returns>
        private async Task List(string path, StreamWriter writer)
        {
            if (!Directory.Exists(path))
            {
                await writer.WriteLineAsync("-1");
                return;
            }

            var files = Directory.GetFiles(path);
            var directories = Directory.GetDirectories(path);
            //    await writer.WriteLineAsync(Convert.ToString(directories.Length + files.Length));

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
        /// Comand get.
        /// </summary>
        /// <param name="path">Path to the file.</param>
        /// <param name="writer">Stream writer</param>
        /// <returns><size: Long> <content: Bytes></returns>
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
    }
}
