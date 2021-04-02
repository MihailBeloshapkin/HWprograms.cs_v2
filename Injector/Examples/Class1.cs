using System;

namespace Examples
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
        public C() { }
    }

    public class D
    {
        public D() { }
    }

    public class X
    {
        public X(Y p) { }
    }


    public class Y
    {
        public Y(X p) { }
    }

}
