using System;
using System.Collections.Generic;
using System.Text;

namespace HW3T1
{
    /// <summary>
    /// My Tsak interface.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public interface IMyTask<out TResult>
    {
        /// <summary>
        /// Is current task completed.
        /// </summary>
        bool IsCompleted { get; }

        /// <summary>
        /// Result.
        /// </summary>
        TResult Result { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TNewResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> func);
    }
}
