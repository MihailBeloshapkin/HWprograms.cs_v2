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

        private bool AlreadyCountet = false; 

        /// <summary>
        /// Calculate result.
        /// </summary>
        /// <returns>Result of the calculation.</returns>
        public T Get()
        {
            if (!AlreadyCountet)
            {
                result = supplier();
                supplier = null;
                AlreadyCountet = true;
            }
            
            return result;
        }
    }
}
