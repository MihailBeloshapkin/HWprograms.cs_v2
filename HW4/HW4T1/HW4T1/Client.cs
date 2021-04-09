using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace HW4T1
{
    /// <summary>
    /// Client realization for our FTP server.
    /// </summary>
    public class Client
    {
        private readonly int port;
        private readonly string host;

        public Client(string host, int port)
        {
            this.host = host;
            this.port = port;
        }

        /// <summary>
        /// Get the file from the path.
        /// </summary>
        public async Task Get(string path, string destination)
        {
            using var client = new TcpClient(host, port);
            using var stream = client.GetStream();
            using var reader = new StreamReader(stream);
            using var writer = new StreamWriter(stream) { AutoFlush = true };
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

            var sizeLong = long.Parse(size);
            string directoryPath = $@"{Directory.GetCurrentDirectory()}\{destination}";
            Directory.CreateDirectory(directoryPath);
            var fileName = Path.GetFileName(path);
            using var fileStream = File.Create($@"{directoryPath}\{fileName}");

            const int maxBufferSize = 81920;
            var buffer = new byte[maxBufferSize];
            while (sizeLong > 0)
            {
                var currentBufferSize = sizeLong > maxBufferSize ? maxBufferSize : (int)sizeLong;
                await stream.ReadAsync(buffer, 0, currentBufferSize);
                await fileStream.WriteAsync(buffer, 0, currentBufferSize);
                sizeLong -= maxBufferSize;
            }
            await reader.ReadLineAsync();
            client.Dispose();
            stream.Dispose();
            reader.Dispose();
            writer.Dispose();
        }
        
        /// <summary>
        /// Get list of directories and files in the path.
        /// </summary>
        public async Task<List<(string, bool)>> List(string path)
        {
            using var client = new TcpClient(host, port);
            using var stream = client.GetStream();
            using var writer = new StreamWriter(stream) { AutoFlush = true };
            using var reader = new StreamReader(stream);

            await writer.WriteLineAsync($"1 {path}");
            var response = await reader.ReadLineAsync();
            if (response == "-1")
            {
                throw new Exception("Directory not found");
            }

            var responceSplit = response.Split(' ');
            var size = int.Parse(responceSplit[0]);

            var result = new List<(string, bool)>();

            for (int iter = 0; iter * 2 + 2 < responceSplit.Length; iter++)
            {
                var fullPath = responceSplit[iter * 2 + 1];
                var isDir = Convert.ToBoolean(responceSplit[iter * 2 + 2]);
                result.Add((fullPath, isDir));
            }
            client.Dispose();
            stream.Dispose();
            reader.Dispose();
            writer.Dispose();
            return result;
        }
    }
}
