using System;
using System.Reflection;
using System.Collections.Generic;

namespace Injector
{
    class Program
    {
        public class A
        {
            public A(B x) { }
        }

        public class B
        {
            public B(C x, D y) { }
        }

        public class C
        {
            public C() {}
        }

        public class D
        {
            public D() { }
        }


        static void Main(string[] args)
        {
            var objectA = Reflect.Initialize(typeof(A), new List<Type> { typeof(B), typeof(C), typeof(D) });
        }
    }
}
