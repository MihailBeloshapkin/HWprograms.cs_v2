using System;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;

namespace HW1T1
{
    class Program
    {
        private void Display(int[,] matrix)
        { 
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); i++)
                {
                    Console.Write("{0, 3}", matrix[i, j]);
                }
                Console.WriteLine();
            }
        }

        private int MultiplyRowAndColumn(int[,] matrix1, int[,] matrix2, int i, int j)
        {
            int result = 0;

            for (int iter = 0; iter < matrix1.GetLength(0); iter++)
            {

            }

            return result;
        }

        private void Multiplication(int[,] matrix1, int[,] matrix2)
        {
            int[,] matrix = new int[matrix1.GetLength(0), matrix2.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {

                }
            }

            
        }

        static void Main(string[] args)
        {
            int[,] matrix1 = new int[2, 3] { {1, 4, 2 }, {7, 2, 5 } };
            int[,] matrix2 = new int[3, 2] { { 0, 3 }, { 1, 7 }, { 5, 3 } };

            var matrix = new Matrix("../../../matrix1.txt");
            Console.WriteLine("Hello World!");
        }
    }
}
