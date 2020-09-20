using System;
using System.Collections.Generic;
using System.Text;

namespace HW2T1
{
    public class SimpleLazy<T> : ILazy<T>
    {
        T result = default(T);

        public T Get()
        {
            return result;
        }
    }
}
