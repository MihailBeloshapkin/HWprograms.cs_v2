using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Gui
{
    /// <summary>
    /// Simple client
    /// </summary>
    public class Client
    {
        private int port;
        private string host;
        //    private readonly TcpClient client;
        //    private readonly Stream stream;
        //    private readonly StreamReader reader;
        //    private readonly StreamWriter writer;

        /// <summary>
        /// Client constructor.
        /// </summary>
        /// <param name="host">Host name</param>
        /// <param name="port">Port name</param>
        public Client(string host, int port)
        {
            this.host = host;
            this.port = port;
            //    this.client = new TcpClient(host, port);
            //    this.stream = client.GetStream();
            //    this.reader = new StreamReader(stream);
            //    this.writer = new StreamWriter(stream) { AutoFlush = true };
        }

        /// <summary>
        /// Requests file downloading from the server.
        /// </summary>
        /// <param name="path">Path to the file.</param>
        /// <returns><size: Long> <content: Bytes></returns>
        public async Task Get(string path, string destination, string fileName)
        {
            var client = new TcpClient(host, port);
            var stream = client.GetStream();
            var reader = new StreamReader(stream);
            var writer = new StreamWriter(stream) { AutoFlush = true };
            await writer.WriteLineAsync($"2 {path}");

            var size = new char[long.MaxValue.ToString().Length + 1];
            await reader.ReadAsync(size, 0, 2);
            if (size[0] == '-')
            {
                throw new ArgumentException();
            }
            var index = 1;
            while (size[index] != ' ')
            {
                index++;
                await reader.ReadAsync(size, index, 1);
            }

            var size_long = long.Parse(size);
            string directoryPath = $@"{Directory.GetCurrentDirectory()}\{destination}";
            Directory.CreateDirectory(directoryPath);
            using var fileStream = File.Create($@"{directoryPath}\{fileName}");

            const int maxBufferSize = 81920;
            var buffer = new byte[maxBufferSize];
            while (size_long > 0)
            {
                var currentBufferSize = size_long > maxBufferSize ? maxBufferSize : (int)size_long;
                await stream.ReadAsync(buffer, 0, currentBufferSize);
                await fileStream.WriteAsync(buffer, 0, currentBufferSize);
                size_long -= maxBufferSize;
            }
            await reader.ReadLineAsync();
        }


        /// <summary>
        /// Requests list of files in server's directory
        /// </summary>
        /// <param name="path">Path to the file.</param>
        /// <returns><size: Int> (<name: String> <isDir: Boolean>)</returns>
        public async Task<List<(string, bool)>> List(string path)
        {
            var client = new TcpClient(host, port);
            using var stream = client.GetStream();
            var writer = new StreamWriter(stream) { AutoFlush = true };
            var reader = new StreamReader(stream);

            await writer.WriteLineAsync($"1 {path}");
            var responce = await reader.ReadLineAsync();
            if (responce == "-1")
            {
                throw new Exception("Directory not found");
            }

            var responceSplit = responce.Split(' ');
            var size = int.Parse(responceSplit[0]);

            var result = new List<(string, bool)>();

            for (int iter = 0; iter * 2 + 2 < responceSplit.Length; iter++)
            {
                var fullPath = responceSplit[iter * 2 + 1];
             //   var isDir = responceSplit[iter * 2 + 2];
                var isDir = Convert.ToBoolean(responceSplit[iter * 2 + 2]);
                result.Add((fullPath, isDir));
            }
            return result;
        }


    }
}