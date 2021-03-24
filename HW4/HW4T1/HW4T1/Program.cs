using System;

namespace HW4T1
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new Server("127.0.0.1", 8888);
            var client = new Client("127.0.0.1", 8888);
        }
    }
}
