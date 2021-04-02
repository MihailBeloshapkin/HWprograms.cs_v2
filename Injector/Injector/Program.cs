using System;
using System.Reflection;
using System.Collections.Generic;

namespace Injector
{
    class Program
    {
        public class A
        {
            public A(Interface1 x) { }
        }

        public class B : Interface1 { }

        public interface Interface1
        {

        }

        static void Main(string[] args)
        {
            var objectA = Reflect.Injector<A>(new List<Type> { typeof(B)});
        }
    }
}
