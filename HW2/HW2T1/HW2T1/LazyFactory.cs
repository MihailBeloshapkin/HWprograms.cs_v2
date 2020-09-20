using System;
using System.Collections.Generic;
using System.Text;

namespace HW2T1
{
    public class LazyFactory<T>
    {
        public static ILazy<T> CreateSimpleLazy(Func<T> supplier)
        {
            return new SimpleLazy<T>();
        }

        /// <summary>
        /// Create alternative lazy class.
        /// </summary>
        public static ILazy<T> CreateAlternativeLazy(Func<T> supplier)
        {
            return new AlternativeLazy<T>();
        }
    }
}
