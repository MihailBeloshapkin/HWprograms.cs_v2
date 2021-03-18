using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;


namespace Client
{

    // This class contains ftp client methods.
    public class Client : IDisposable
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

        
        public async Task<IEnumerable<(string name, bool isDir)>> List(string path)
        {
            await writer.WriteLineAsync($"1{path}");
            var response = await reader.ReadLineAsync();
            if (response == "-1")
            {
                throw new InvalidOperationException();
            }

            var splittedResponse = response.Split(' ');
            var size = int.Parse(splittedResponse[0]);

            var result = new List<(string, bool)>();

            for (int iter = 0; iter < size; iter++)
            {
                result.Add((splittedResponse[iter * 2 + 1], bool.Parse(splittedResponse[iter * 2 + 2])));
            }

            return result;
        }

        public async Task Get(string pathToFile, string downloadTo, string fileName)
        {
            await writer.WriteLineAsync($"2 {pathToFile}");
            var response = await reader.ReadLineAsync();
            var splitResponse = response.Split(' ', 2);

            if (!long.TryParse(splitResponse[0], out var size))
            {
                throw new Exception();
            }

            await this.DownloadFile(size, downloadTo, pathToFile);

        }

        private async Task DownloadFile(long size, string pathForDownload, string fileName)
        {
            string directoryPath = $@"{Directory.GetCurrentDirectory()}\{pathForDownload}";
            Directory.CreateDirectory(directoryPath);
            using var fileStream = File.Create($@"{directoryPath}\{fileName}");

            const int bufferSizeMax = 81920;
            var buffer = new byte[bufferSizeMax];

            while (size > 0)
            {
                var currentBufferSize = size > bufferSizeMax ? bufferSizeMax : (int)size;
                await stream.ReadAsync(buffer, 0, currentBufferSize);
                await fileStream.WriteAsync(buffer, 0, currentBufferSize);
                size -= currentBufferSize;
            }

            await reader.ReadLineAsync();
        }

        public void Dispose()
        {
            writer.Dispose();
            reader.Dispose();
        }
    }
}
