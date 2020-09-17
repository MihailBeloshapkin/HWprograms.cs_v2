using System;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Diagnostics;
using System.Collections.Generic;


namespace HW1T1
{
    class Program
    {
        static void Main(string[] args)
        {
            var matrix1 = new Matrix("../../../matrix1.txt");
            var matrix2 = new Matrix("../../../matrix2.txt");

            var bigMatrix1 = new Matrix("../../../bigMatrix1.txt");
            var bigMatrix2 = new Matrix("../../../bigMatrix2.txt");

            var sw1 = new Stopwatch();
            var sw2 = new Stopwatch();
            var sw3 = new Stopwatch();
            var sw4 = new Stopwatch();

            sw1.Start();
            Multiplication.SimpleMultiplication(matrix1, matrix2);
            sw1.Stop();

            sw2.Start();
            Multiplication.ParallelMultiplication(matrix1, matrix2);
            sw2.Stop();

            sw3.Start();
            Multiplication.SimpleMultiplication(bigMatrix1, bigMatrix2);
            sw3.Stop();

            sw4.Start();
            Multiplication.ParallelMultiplication(bigMatrix1, bigMatrix2);
            sw4.Stop();

            Console.Write($"Time of the simple multiplication of two simple matrices: {sw1.ElapsedMilliseconds / 100.0}");
            Console.WriteLine();
            Console.Write($"Time of the parallel multiplication of two simple matrices: {sw2.ElapsedMilliseconds / 100.0}");
            Console.WriteLine();
            Console.WriteLine();
            Console.Write($"Time of the simple multiplication of two big matrices:{sw3.ElapsedMilliseconds / 100.0}");
            Console.WriteLine();
            Console.Write($"Time of the parallel multiplication of two big matrices:{sw4.ElapsedMilliseconds / 100.0}");


        }
    }
}
