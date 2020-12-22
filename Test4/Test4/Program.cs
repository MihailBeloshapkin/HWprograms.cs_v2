using System;
using System.Diagnostics;
using System.Net;

namespace Test4
{
    class Program
    {
        private const int arraySize = 100000000;

        static void Main(string[] args)
        {
            var stopwatch1 = new Stopwatch();
            var stopwatch2 = new Stopwatch();

            int[] array = new int[arraySize];
            for (int iter = 0; iter < array.Length; iter++)
            {
                array[iter] = array.Length - iter;
            }

            stopwatch1.Start();
            QuickSort.SimpleQuickSort(array, 0, array.Length - 1);
            stopwatch1.Stop();

            

            for (int iter = 0; iter < array.Length; iter++)
            {
                array[iter] = array.Length - iter;
            }

            stopwatch2.Start();
            QuickSort.ParallelQuickSort(array, 0, array.Length - 1);
            stopwatch2.Stop();

            Console.WriteLine($"Simple quick sort takes {stopwatch1.ElapsedMilliseconds} milliseconds");
            Console.WriteLine($"Parallel quick sort takes {stopwatch2.ElapsedMilliseconds} milliseconds");
        }
    }
}
