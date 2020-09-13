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
            var result = Multiplication.SimpleMultiplication(matrix1, matrix2);
            result.Display();
        }
    }
}
