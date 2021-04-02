using System;
using System.Diagnostics;
using System.Net;

namespace Test4
{
    class Program
    {
        private const int bigArraySize = 100000000;
        private const int arraySize = 10000;

        static void Main(string[] args)
        {
            var stopwatch1 = new Stopwatch();
            var stopwatch2 = new Stopwatch();

            int[] bigArray = new int[bigArraySize];
            for (int iter = 0; iter < bigArray.Length; iter++)
            {
                bigArray[iter] = bigArray.Length - iter;
            }

            stopwatch1.Start();
            QuickSort.SimpleQuickSort(bigArray, 0, bigArray.Length - 1);
            stopwatch1.Stop();

            

            for (int iter = 0; iter < bigArray.Length; iter++)
            {
                bigArray[iter] = bigArray.Length - iter;
            }

            stopwatch2.Start();
            QuickSort.ParallelQuickSort(bigArray, 0, bigArray.Length - 1);
            stopwatch2.Stop();

            Console.WriteLine($"Simple quick sort takes {stopwatch1.ElapsedMilliseconds} milliseconds");
            Console.WriteLine($"Parallel quick sort takes {stopwatch2.ElapsedMilliseconds} milliseconds");

            var stopwatch3 = new Stopwatch();
            var stopwatch4 = new Stopwatch();

            int[] array = new int[bigArraySize];
            for (int iter = 0; iter < array.Length; iter++)
            {
                array[iter] = array.Length - iter;
            }

            stopwatch3.Start();
            QuickSort.SimpleQuickSort(array, 0, array.Length - 1);
            stopwatch3.Stop();



            for (int iter = 0; iter < array.Length; iter++)
            {
                array[iter] = array.Length - iter;
            }

            stopwatch4.Start();
            QuickSort.ParallelQuickSort(array, 0, bigArray.Length - 1);
            stopwatch4.Stop();
            Console.WriteLine($"In case if array size is ");
            Console.WriteLine($"Simple quick sort takes {stopwatch3.ElapsedMilliseconds} milliseconds");
            Console.WriteLine($"Parallel quick sort takes {stopwatch4.ElapsedMilliseconds} milliseconds");
        }
    }
}
