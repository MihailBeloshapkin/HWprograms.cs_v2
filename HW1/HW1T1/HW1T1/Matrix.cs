using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace HW1T1
{
    /// <summary>
    /// This class contains matrix and its functions.
    /// </summary>
    public class Matrix
    {
        public int[,] matrix { private set; get; } = null;

        public Matrix(string fileName)
        {
            this.LoadAndCreateMatrix(fileName);
        }

        /// <summary>
        /// Load matrix from txt file and create.
        /// </summary>
        public void LoadAndCreateMatrix(string fileName)
        {
            var queueOfRows = new Queue<string>();

            using (var sr = new StreamReader(fileName))
            {
                while (!sr.EndOfStream)
                {
                    queueOfRows.Enqueue(sr.ReadLine());
                }
            }

            var currentRow = queueOfRows.Dequeue();
            var currentIntegerRow = new Queue<int>();
            int jiter = 0;

            while (jiter < currentRow.Length)
            {
                if (!char.IsDigit(currentRow[jiter]) && currentRow[jiter] != ' ' && currentRow[jiter] != '\n' && currentRow[jiter] != '\r')
                {
                    throw new Exception();
                }

                string number = null;

                while (jiter < currentRow.Length && currentRow[jiter] != ' ')
                {
                    number += currentRow[jiter];
                    jiter++;
                }

                if (number != null)
                {
                    currentIntegerRow.Enqueue(int.Parse(number));

                }
                jiter++;
            }

            this.matrix = new int[queueOfRows.Count + 1, currentIntegerRow.Count];

            for (int i = 0; currentIntegerRow.Count > 0; i++)
            {
                matrix[0, i] = currentIntegerRow.Dequeue();
            }

            for (int i = 1; queueOfRows.Count > 0; i++)
            {
                currentRow = queueOfRows.Dequeue();

                int iter = 0;
                int rowIndex = 0;

                while (iter < currentRow.Length)
                {
                    string number = null;
                    
                    if (!char.IsDigit(currentRow[iter]) && currentRow[iter] != ' ' && currentRow[iter] != '\n' && currentRow[iter] != '\r')
                    {
                        throw new Exception();
                    }

                    while (iter < currentRow.Length && currentRow[iter] != ' ')
                    {
                        number += currentRow[iter];
                        iter++;
                    }

                    if (number != null)
                    {
                        this.matrix[i, rowIndex] = int.Parse(number);
                    }

                    rowIndex++;
                    iter++;
                }
            }

        }


    }
}
