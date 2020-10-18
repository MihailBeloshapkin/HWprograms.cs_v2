using NUnit.Framework;
using System;

namespace HW3T1
{
    public class Tests
    {
        private MyThreadPool threadPool;
        private Object locker = new object();

        [SetUp]
        public void SetUp()
        {
            threadPool = new MyThreadPool(Environment.ProcessorCount);
        }

        [Test]
        public void SimpleTest()
        {
            var task = threadPool.Submit<int>(() => 2 * 2);
            Assert.AreEqual(4, task.Result);
            Assert.IsTrue(task.IsCompleted);
        }

        [Test]
        public void SimpleContinueWithTest()
        {
            threadPool = new MyThreadPool(1);
            var task = threadPool.Submit<int>(() => 2 * 2).ContinueWith(x => x * 3);
            Assert.AreEqual(12, task.Result);
            Assert.IsTrue(task.IsCompleted);
        }

        [Test]
        public void NullFunctionTest()
        {
            Assert.Throws<ArgumentNullException>(() => threadPool.Submit<int>(null));
        }
    }
}