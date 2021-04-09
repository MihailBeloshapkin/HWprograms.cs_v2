using System;
using System.Reflection;
using System.Collections.Generic;

namespace Injector
{
    class Program
    {
        public class A
        {
            public A(B x, C y) { }
        }

        public class B
        {
            public B(D x) { }
        }

        public class C
        {
            public C(D x) {}
        }

        public class D
        {
            public D() { }
        }

        public class X
        {
            public X(Y a, Y b) { }
        }

        public class Y
        {
            public Y() { }
        }


        static void Main(string[] args)
        {
            var objectA = Reflect.Initialize(typeof(A), new List<Type> { typeof(B), typeof(C), typeof(D) });
        }
    }
}
