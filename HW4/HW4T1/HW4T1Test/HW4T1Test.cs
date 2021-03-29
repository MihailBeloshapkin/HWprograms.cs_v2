using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using HW4T1;

namespace HW4T1Test
{
    public class Tests
    {
        Server server;
        Client client;

        [SetUp]
        public void Setup()
        {
            server = new Server("127.0.0.1", 8888);
            client = new Client("127.0.0.1", 8888);
            _ = server.Process();
        }

        [Test]
        public async Task ListTest()
        {
            var data = await client.List("../../../../HW4T1Test/testData");
            Assert.AreEqual(2, data.Item1);
            
        }       
    }
}