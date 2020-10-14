using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace HW3T1
{
    public class MyTask<TResult> : IMyTask<TResult>
    {
        TResult result;

        public TResult Result = default(TResult);

        public bool IsCompleted = false;

        IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> func)
        {

        }
    }
}
