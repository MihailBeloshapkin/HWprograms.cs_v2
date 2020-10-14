using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;

namespace HW3T1
{
    public class MyTask<TResult> : IMyTask<TResult>
    {
        public MyTask(Func<TResult> func)
        {
            this.func = func;
        }
            

        TResult result;

        public TResult Result { get; } = default(TResult);

        public bool IsCompleted { get; set; } = false;

        private MyThreadPool threadPool = null;

        private Func<TResult> func;

        private AggregateException aggregateException = null;

        public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> func)
        {
            return this;
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
        }

        IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> newFunc)
        {

        }
    }
}
