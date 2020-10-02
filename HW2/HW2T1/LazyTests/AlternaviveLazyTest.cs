using NUnit.Framework;
using System.Threading;

namespace LazyTests
{
    using HW2T1;
    using System;
    using System.Runtime.InteropServices.ComTypes;

    public class AlternativeLazyTest
    {
        
        [Test]
        public void CommonTest()
        {
            var lazy = LazyFactory<int>.CreateAlternativeLazy(() => 30);
            Assert.AreEqual(30, lazy.Get());
        }

        [Test]
        public void StringResultTest()
        {
            var lazy = LazyFactory<string>.CreateAlternativeLazy(() => "result");
            Assert.AreEqual("result", lazy.Get());
        }

        [Test]
        public void SupplierWithNullResultTest()
        {
            var lazy = LazyFactory<object>.CreateAlternativeLazy(() => null);
            Assert.AreEqual(null, lazy.Get());
        }

        [Test]
        public void ParallelCalculationTest()
        {
            var result = 0;
            var rightResult = 10000;
            var threads = new Thread[Environment.ProcessorCount];

            var lazy = LazyFactory<int>.CreateAlternativeLazy(() =>
            {
                for (int i = 0; i < rightResult; i++)
                {
                    Interlocked.Increment(ref result);
                }

                return result;
            });

            for (int iter = 0; iter < threads.Length; iter++)
            {
                var localI = iter;
                threads[localI] = new Thread(() => lazy.Get());
            }

            foreach (var thread in threads)
            {
                thread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            Assert.AreEqual(rightResult, result);
        }
    }
}