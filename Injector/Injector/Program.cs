using System;
using System.Reflection;
using System.Collections.Generic;

namespace Injector
{
    class Program
    {
        public class A
        {
            public A(C x, B y) { }
        }

        public class B
        {
            public B(C x) { }
        }

        public class C
        {
            public C() {}
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
            var objectA = Reflect.Initialize(typeof(X), new List<Type> { typeof(Y), typeof(Y) });
        }
    }
}
