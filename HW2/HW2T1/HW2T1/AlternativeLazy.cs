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
        public AlternativeLazy(Func<T> supplier)
        {
            this.supplier = supplier;
        }

        private T result = default(T);

        private Func<T> supplier;

        private bool AlreadyCounted { get; set; } = false;

        private Object locker = new Object();

        public T Get()
        {
            if (!AlreadyCounted)
            {
                lock(locker)
                {
                    result = supplier();
                    AlreadyCounted = true;
                }
            }

            return result;
        }
    }
}
