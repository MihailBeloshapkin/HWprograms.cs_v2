using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace HW1T1
{
    public class Matrix
    {
        public int[,] matrix { private set; get; }

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

            for (int i = 0; i < queueOfRows.Count; i++)
            {

            }
        }
    }
}
