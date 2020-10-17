using NUnit.Framework;
using System;

namespace HW3T1
{
    public class Tests
    {
        MyThreadPool threadPool;

        [Test]
        public void SimpleTest()
        {
            threadPool = new MyThreadPool(2);
            var task = threadPool.Submit<int>(() => 2 + 3);
            Assert.AreEqual(5, task.Result);
        }
    }
}