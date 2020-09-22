using System;
using System.Collections.Generic;
using System.Text;

namespace HW2T1
{
    public interface ILazy<T>
    {
        public T Get();
    }
}
