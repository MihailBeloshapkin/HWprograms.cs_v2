using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

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
    }
}
