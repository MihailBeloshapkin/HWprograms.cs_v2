using NUnit.Framework;

namespace LazyTests
{
    using HW2T1;

    public class SimpleLazyTest
    {
        [Test]
        public void CommonTest()
        {
            var lazy = LazyFactory<int>.CreateSimpleLazy(() => 30);
            Assert.AreEqual(30, lazy.Get());
        }

        [Test]
        public void ChangingResultTest()
        {
            int variable = 30;
            var lazy = LazyFactory<int>.CreateSimpleLazy(() => variable);
            var result1 = lazy.Get();
            variable++;
            var result2 = lazy.Get();
            Assert.AreEqual(30, result1, result2);
        }

        [Test]
        public void StringResultTest()
        {
            var lazy = LazyFactory<string>.CreateSimpleLazy(() => "result");
            Assert.AreEqual("result", lazy.Get());
        }

        [Test]
        public void SupplierWithNullResultTest()
        {
            var lazy = LazyFactory<object>.CreateSimpleLazy(() => null);
            Assert.AreEqual(null, lazy.Get());
        }
    }
}