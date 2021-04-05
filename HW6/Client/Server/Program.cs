using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

namespace Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var server = new HW6T1.Server("127.0.0.1", 8888);
            _ = server.Process();
        }
    }
}
