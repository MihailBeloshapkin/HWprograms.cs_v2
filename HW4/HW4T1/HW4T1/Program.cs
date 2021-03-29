using System;
using System.Threading.Tasks;

namespace HW4T1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var server = new Server("127.0.0.1", 8888);
            
            var client = new Client("127.0.0.1", 8888);
            _ = server.Process();
            var command = Console.ReadLine();  // ../../../../HW4T1Test/testData
            
            switch (command)
            {
                case "1":
                    await client.Get("../../../../Data/data.txt", "../../../../destination");
                    break;
                case "2":
                    var list = await client.List("../../../../HW4T1");
                    Console.WriteLine(list.Item1);
                    foreach (var data in list.Item2)
                    {
                        var (name, isDir) = data;

                        Console.Write(name);
                        Console.Write(isDir ? " true" : " false");
                        Console.WriteLine();
                    }
                    break;
            }
        } // ../../../../HW4T1
    }
}
