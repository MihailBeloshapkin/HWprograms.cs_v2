using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace HW3T1
{
    public class MyTask<TResult> : IMyTask<TResult>
    {
        public MyTask(Func<TResult> func)
        {
            this.func = func;
        }
            

        TResult result;

        public TResult Result = default(TResult);

        public bool IsCompleted = false;

        private Func<TResult> func;

        private AggregateException aggregateException = null;

        /// <summary>
        /// Apply function to result.
        /// </summary>
        private void Execute()
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
