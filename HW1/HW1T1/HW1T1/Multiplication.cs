using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace HW1T1
{
    public class Multiplication
    {

        private static int MultiplyRowAndColumn(Matrix matrix1, Matrix matrix2, int i, int j)
        {
            int result = 0;

            for (int iter = 0; iter < matrix1.CountOfColumns; iter++)
            {
                result += matrix1.matrix[i, iter] * matrix2.matrix[iter, j];
            }

            return result;
        }

        public static Matrix SimpleMultiplication(Matrix matrix1, Matrix matrix2)
        {

            if (matrix1.CountOfColumns != matrix2.CountOfRows)
            {
                throw new Exception();
            }

            var result = new Matrix(matrix1.CountOfRows, matrix2.CountOfColumns);

            for (int i = 0; i < result.CountOfRows; i++)
            {
                for (int j = 0; j < result.CountOfColumns; j++)
                {
                    result.matrix[i, j] = MultiplyRowAndColumn(matrix1, matrix2, i, j);
                }
            }

            return result;
        }

        /// <summary>
        /// The same algorithm as in the SimpleMultiplication, but executes by multithreading.
        /// </summary>
        /// <returns>Result of the multiplication.</returns>
        public static Matrix ParallelMultiplication(Matrix matrix1, Matrix matrix2)
        {
            if (matrix1.CountOfColumns != matrix2.CountOfRows)
            {
                throw new Exception();
            }

            var result = new Matrix(matrix1.CountOfRows, matrix2.CountOfColumns);
            var threads = new Thread[Environment.ProcessorCount];

            var chunkSize = result.CountOfRows / threads.Length + 1;

            for (int iter = 0; iter < threads.Length; iter++)
            {
                var localI = iter;
                threads[localI] = new Thread(() =>
                {
                    for (int i = localI * chunkSize; i < (localI + 1) * chunkSize && i < result.CountOfRows; i++)
                    {
                        for (int j = 0; j < result.CountOfColumns; j++)
                        {
                            result.matrix[i, j] = MultiplyRowAndColumn(matrix1, matrix2, i, j);
                        }
                    }
                }
               );
            }

            foreach (var thread in threads)
            {
                thread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            return result;
        }
    }
}
