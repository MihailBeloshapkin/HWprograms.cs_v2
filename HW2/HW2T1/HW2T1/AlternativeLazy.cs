using System;
using System.Collections.Generic;
using System.Text;

namespace HW2T1
{
    /// <summary>
    /// Alternative Lazy realization.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AlternativeLazy<T> : ILazy<T>
    {
        private T result = default(T);

        public T Get()
        {
            return result;
        }
    }
}
