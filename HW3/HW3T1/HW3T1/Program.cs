using System;
using System.Collections.Generic;
using System.Text;

namespace HW3T1
{
    class Program
    {
        static void Main(string[] args)
        {
            var threadPool = new MyThreadPool(2);
            
            var task = threadPool.Submit<int>(() => 2 * 2).ContinueWith<int>(x => x * 5);
            var task_0 = threadPool.Submit<int>(() => 2 + 3);
        }
    }
}
