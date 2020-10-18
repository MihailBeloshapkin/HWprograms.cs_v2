using NUnit.Framework;
using System;

namespace HW3T1
{
    public class Tests
    {
        private MyThreadPool threadPool;
        private Object locker = new object();

        [Test]
        public void SimpleTest()
        {
            threadPool = new MyThreadPool(1);
            var task = threadPool.Submit<int>(() => 2 * 2);
            Assert.IsTrue(task.IsCompleted);
            Assert.AreEqual(4, task.Result);
        }
    }
}