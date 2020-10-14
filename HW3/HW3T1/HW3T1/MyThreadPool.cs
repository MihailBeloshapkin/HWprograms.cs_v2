using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            Tasks = new ThreadQueue<Action>();
            this.Start();
        }

        private int countOfThreads = 0;

        private Thread[] Threads = null;

        private readonly CancellationTokenSource cancellationTokenSource = null;

        private readonly ThreadQueue<Action> Tasks = null;

        private void Start()
        {
            for (int iter = 0; iter < Threads.Length; iter++)
            {
                Threads[iter] = new Thread(() => Execute(cancellationTokenSource.Token));
                Threads[iter].Start();
            }
        }

        public void Execute(CancellationToken cancellationToken)
        {
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                {

                }
                else 
                { 

                }
            }
        }

        /// <summary>
        /// Interrupts all executing tasks.
        /// </summary>
        public void ShutDown()
            => cancellationTokenSource.Cancel();

        public IMyTask<TResult> Submit<TResult>(Func<TResult> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException();
            }

            var newTask = 
            this.Tasks.Enqueue();
            return null;
        }
    }
}
