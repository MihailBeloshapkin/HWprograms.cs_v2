using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using System.Transactions;

namespace HW1T1
{
    /// <summary>
    /// This class contains matrix and its functions.
    /// </summary>
    public class Matrix
    {
        public int[,] CurrentMatrix { private set; get; } = null;

        public int CountOfRows { get => CurrentMatrix.GetLength(0); }

        public int CountOfColumns { get => CurrentMatrix.GetLength(1); }

        public Matrix(int countOfRows, int countOfColumns)
        {
            if (countOfRows < 1 || countOfColumns < 1)
            {
                throw new ArgumentException();
            }
            this.CurrentMatrix = new int[countOfRows, countOfColumns];
        }

        public Matrix(string fileName)
        {
            this.LoadAndCreateMatrix(fileName);
        }

        public Matrix(int[,] matrix)
        {
            this.CurrentMatrix = matrix;
        }

        /// <summary>
        /// Display current matrix to a console.
        /// </summary>
        public void Display()
        {
            for (int i = 0; i < CurrentMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < CurrentMatrix.GetLength(1); j++)
                {
                    Console.Write("{0, 3}", this.CurrentMatrix[i, j]);
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Load matrix from txt file and create.
        /// </summary>
        private void LoadAndCreateMatrix(string fileName)
        {
            var queueOfRows = new Queue<string>();

            using (var sr = new StreamReader(fileName))
            {
                while (!sr.EndOfStream)
                {
                    queueOfRows.Enqueue(sr.ReadLine());
                }
            }

            var currentIntegerRow = new Queue<int>();
            
       
            for (int i = 0; queueOfRows.Count > 0; i++)
            {
                var currentRow = queueOfRows.Dequeue();

                int iter = 0;
                int rowIndex = 0;

                while (iter < currentRow.Length)
                {
                    string number = null;
                    
                    if (!char.IsDigit(currentRow[iter]) && currentRow[iter] != ' ' && currentRow[iter] != '\n' && currentRow[iter] != '\r')
                    {
                        throw new UnrecognisedCharException();
                    }

                    while (iter < currentRow.Length && currentRow[iter] != ' ')
                    {
                        number += currentRow[iter];
                        iter++;
                    }

                    if (number != null)
                    {
                        if (CurrentMatrix == null)
                        {
                            currentIntegerRow.Enqueue(int.Parse(number));
                        }
                        else 
                        {
                            this.CurrentMatrix[i, rowIndex] = int.Parse(number);
                        }
                    }

                    rowIndex++;
                    iter++;
                }

                if (this.CurrentMatrix == null)
                {
                    this.CurrentMatrix = new int[queueOfRows.Count + 1, currentIntegerRow.Count];
                
                    for (int i0 = 0; currentIntegerRow.Count > 0; i0++)
                    {
                        CurrentMatrix[0, i0] = currentIntegerRow.Dequeue();
                    }
                }
            }
        }

        /// <summary>
        /// Check that the input matrix is equal to the current. 
        /// </summary>
        public bool IsEqualTo(int[,] matrix)
        {
            if (this.CountOfRows != matrix.GetLength(0) || this.CountOfColumns != matrix.GetLength(1))
            {
                return false;
            }

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (this.CurrentMatrix[i, j] != matrix[i, j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
