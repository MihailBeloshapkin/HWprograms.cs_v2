using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
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
            this.cancellationTokenSource = new CancellationTokenSource();
            this.Tasks = new ConcurrentQueue<Action>();
            this.Start();
        }

        private int countOfThreads = 0;

        private Thread[] Threads = null;

        private readonly CancellationTokenSource cancellationTokenSource = null;

        private readonly ConcurrentQueue<Action> Tasks = null;

        private Object locker = new Object();

        /// <summary>
        /// Start all threads.
        /// </summary>
        private void Start()
        {
            for (int iter = 0; iter < Threads.Length; iter++)
            {
                Threads[iter] = new Thread(() => Execute(cancellationTokenSource.Token));
                Threads[iter].Start();
            }
        }

        /// <summary>
        /// Execute current.
        /// </summary>
        public void Execute(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (this.Tasks.TryDequeue(out Action task))
                {
                    task.Invoke();
                }
            }
        }

        /// <summary>
        /// Interrupts all executing tasks.
        /// </summary>
        public void Shutdown()
            => cancellationTokenSource.Cancel();

        /// <summary>
        /// Submit new function.
        /// </summary>
        /// <param name="func">Input function</param>
        public IMyTask<TResult> Submit<TResult>(Func<TResult> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException();
            }

            lock (locker)
            {
                var newTask = new MyTask<TResult>(func, this);
                this.Tasks.Enqueue(newTask.Execute);
                return newTask;
            }
        }

        private void SubmitAction<TResult>(Action action)
        {
            if (!cancellationTokenSource.IsCancellationRequested)
            {
                this.Tasks.Enqueue(action);
            }
        }

        /// <summary>
        /// Task and its methods.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        private class MyTask<TResult> : IMyTask<TResult>
        {
            public MyTask(Func<TResult> func, MyThreadPool threadPool)
            {
                this.threadPool = threadPool;
                this.func = func;
                this.submitFunctionsQueue = new Queue<Action>();
            }


            public TResult Result
            {
                get
                {
                    return this.result;
                }
            }

            public bool IsCompleted { get; set; } = false;

            private MyThreadPool threadPool = null;

            private Func<TResult> func;

            private AggregateException aggregateException = null;

            private Queue<Action> submitFunctionsQueue = null;

            private TResult result = default(TResult);

            private Object locker = new Object();

            public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> newFunc)
            {
                if (newFunc == null)
                {
                    throw new ArgumentNullException();
                }

                lock (locker)
                {
                    if (this.IsCompleted)
                    {
                        return threadPool.Submit(() => newFunc(result));
                    }
                    var newTask = new MyTask<TNewResult>(() => newFunc(result), threadPool);
                    this.submitFunctionsQueue.Enqueue(newTask.Execute);
                    return newTask;
                }
            }

            /// <summary>
            /// Apply function to result.
            /// </summary>
            public void Execute()
            {
                try
                {
                    result = this.func();
                }
                catch (Exception e)
                {
                    this.aggregateException = new AggregateException(e);
                }
                finally
                {
                    lock (locker)
                    {
                        this.IsCompleted = true;
                        while (submitFunctionsQueue.Count > 0)
                        {
                            threadPool.SubmitAction<TResult>(submitFunctionsQueue.Dequeue());
                        }
                    }
                    
                }
            }
        }
    }
}

