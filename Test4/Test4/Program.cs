using System;

namespace Test4
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] array = new int[] { 5, 4, 3, 2, 1, 7, 6 };
            QuickSort.ParallelQuickSort(array, 0, array.Length - 1);
            for (int iter = 0; iter < array.Length; iter++)
            {
                Console.Write($"{array[iter]} ");
            }
        }
    }
}
