using System;
using System.Collections.Generic;
using System.Text;

namespace HW3T1
{
    public interface IMyTask<out TResult>
    {
        bool IsCompleted { get; }

        TResult Result { get; }

        IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> func);
    }
}
