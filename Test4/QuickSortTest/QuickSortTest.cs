using NUnit.Framework;

namespace Test4
{
    public class Tests
    {
        private int[] array;

        private int[] sortedArray;

        [SetUp]
        public void Setup()
        {
            array = new int[30];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = array.Length - i;
            }

            sortedArray = new int[30];
            for (int i = 0; i < sortedArray.Length; i++)
            {
                sortedArray[i] = i;
            }
        }

        [Test]
        public void CheckThatSimpleRealizationIsCorrect()
        {
            QuickSort.SimpleQuickSort(array, 0, array.Length - 1);
            for (int iter = 0; iter < array.Length - 1; iter++)
            {
                Assert.IsTrue(array[iter] < array[iter + 1]);
            }
        }

        [Test]
        public void CheckSimpleQuickSortWithSortedArray()
        {
            QuickSort.SimpleQuickSort(array, 0, array.Length - 1);
            for (int iter = 0; iter < array.Length - 1; iter++)
            {
                Assert.IsTrue(sortedArray[iter] < sortedArray[iter + 1]);
            }
        }

        [Test]
        public void CheckThatParallelRealizationIsCorrect()
        {
            QuickSort.ParallelQuickSort(array, 0, array.Length - 1);
            for (int iter = 0; iter < array.Length - 1; iter++)
            {
                Assert.IsTrue(array[iter] < array[iter + 1]);
            }
        }

        [Test]
        public void CheckParallelQuickSortWithSortedArray()
        {
            QuickSort.ParallelQuickSort(sortedArray, 0, array.Length - 1);
            for (int iter = 0; iter < sortedArray.Length - 1; iter++)
            {
                Assert.IsTrue(sortedArray[iter] < sortedArray[iter + 1]);
            }
        }

        [Test]
        public void OneElementArraySimpleQuickSortTest()
        {
            int[] oneElementArray = new int[] { 7 };
            QuickSort.SimpleQuickSort(oneElementArray, 0, oneElementArray.Length - 1);
            Assert.AreEqual(7, oneElementArray[0]);
        }

        [Test]
        public void OneElementArrayParallelQuickSortTest()
        {
            int[] oneElementArray = new int[] { 7 };
            QuickSort.ParallelQuickSort(oneElementArray, 0, oneElementArray.Length - 1);
            Assert.AreEqual(7, oneElementArray[0]);
        }
    }
}