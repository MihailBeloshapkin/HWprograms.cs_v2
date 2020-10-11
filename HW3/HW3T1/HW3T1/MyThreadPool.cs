using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace HW3T1
{
    /// <summary>
    /// Thread pool.
    /// </summary>
    public class MyThreadPool
    {
        public MyThreadPool(int countOfThreads)
        {
            this.countOfThreads = countOfThreads;
            this.Threads = new Thread[countOfThreads];
            cancellationTokenSource = new CancellationTokenSource();
            newTasks = new Queue<Action>();
            this.Start();
        }

        private int countOfThreads = 0;

        private Thread[] Threads = null;

        private readonly CancellationTokenSource cancellationTokenSource = null;

        private readonly Queue<Action> newTasks = null;

        private void Start()
        {
            foreach (var thread in Threads)
            {
            }
        }

        /// <summary>
        /// Interrupts all executing tasks.
        /// </summary>
        public void ShutDown()
            => cancellationTokenSource.Cancel();
    }
}
