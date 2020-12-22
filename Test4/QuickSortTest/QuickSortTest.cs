using NUnit.Framework;

namespace QuickSortTest
{
    public class Tests
    {
        private int[] array;

        [SetUp]
        public void Setup()
        {
            array = new int[7];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = array.Length - i;
            }

        }

        [Test]
        public void CheckThatSimpleRealizationIsCorrect()
        {

        }
    }
}