using System;
using System.Collections.Generic;
using System.Text;

namespace HW2T1
{
    /// <summary>
    /// This interface contains method for lazy computing.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ILazy<T>
    {
        /// <summary>
        /// Calculate.
        /// </summary>
        public T Get();
    }
}
