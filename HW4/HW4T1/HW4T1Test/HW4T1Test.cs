using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using HW4T1;

namespace HW4T1Test
{
    public class Tests
    {
        private Server server;
        private Client client;

        [SetUp]
        public void Setup()
        {
            server = new Server("127.0.0.1", 8888);
            client = new Client("127.0.0.1", 8888);
            _ = server.Process();
        }

        [TearDown]
        public void Teardown()
        {
            server.ShutDown();
            
        }

        [Test]
        public async Task ListTest()
        {
            var data = await client.List("../../../../HW4T1Test/testData");
            Assert.IsTrue(data.Contains((@"../../../../HW4T1Test/testData\folder", true)));
        }
        
        [Test]
        public async Task ListCountTest()
        {
            var data = await client.List("../../../../HW4T1Test/testData");
            Assert.AreEqual(2, data.Count);

        }

        [Test]
        public async Task GetTest()
        {
            await client.Get("../../../../HW4T1Test/testData/file.txt", "../../../../HW4T1Test/testData/folder");
            Assert.IsTrue(File.Exists("../../../../HW4T1Test/testData/folder/file.txt"));
        }
        
        [Test]
        public async Task IncorrectPathTest()
        {
            Assert.ThrowsAsync<ArgumentException>(() => client.Get("randomPath", "randomDestination"));
        }
    }
}