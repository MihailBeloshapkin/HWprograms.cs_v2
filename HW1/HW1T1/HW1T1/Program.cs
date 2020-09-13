using System;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;

namespace HW1T1
{
    class Program
    {
        static void Main(string[] args)
        {
            var matrix1 = new Matrix("../../../matrix1.txt");
            var matrix2 = new Matrix("../../../matrix2.txt");
            var result1 = Multiplication.SimpleMultiplication(matrix1, matrix2);
            var result2 = Multiplication.ParallelMultiplication(matrix1, matrix2);
            result1.Display();
            result2.Display();
        }
    }
}
