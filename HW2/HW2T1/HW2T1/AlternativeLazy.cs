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

        /// <summary>
        /// Calculate result.
        /// </summary>
        /// <returns>Result of the calculation.</returns>
        public T Get()
        {
            if (!AlreadyCounted)
            {
                lock(locker)
                {
                    if (AlreadyCounted)
                    {
                        return result;
                    }

                    result = supplier();
                    supplier = null;
                    AlreadyCounted = true;
                }
            }

            return result;
        }
    }
}
