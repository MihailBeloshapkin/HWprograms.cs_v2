using System;
using System.Collections.Generic;
using System.Text;

namespace HW2T1
{
    /// <summary>
    /// Simple realization of ILazy interface.
    /// </summary>
    public class SimpleLazy<T> : ILazy<T>
    {
        public SimpleLazy(Func<T> supplier)
        {
            this.supplier = supplier;
        }
            
        private T result = default(T);

        private Func<T> supplier;

        private bool AlreadyCounter { get; set; } = false; 

        /// <summary>
        /// Calculate result.
        /// </summary>
        /// <returns>Result of the calculation.</returns>
        public T Get()
        {
            if (!AlreadyCounter)
            {
                result = supplier();
                AlreadyCounter = true;
            }
            
            return result;
        }
    }
}
