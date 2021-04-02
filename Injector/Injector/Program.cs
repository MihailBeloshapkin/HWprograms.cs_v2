using System;
using System.Reflection;
using System.Collections.Generic;

namespace Injector
{

    interface IInfoClass
    {
        double Sum();
        void Info();
        void Set(double d1, double d2);
    }

    // Тестовый класс, содержащий некоторые конструкции
    public class MyTestClass : IInfoClass
    {
        public MyTestClass(double d1, double d2)
        {

        }
        public double Sum()
        {
            return 0.1;
        }

        public void Info()
        {

        }

        public void Set(double d1, double d2)
        {

        }
    }
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

       

        // Данн

        
        static void Initialize(Type rootClass)
        {
            
            ConstructorInfo[] info = rootClass.GetConstructors();
        }

        class Student
        { 
        
        }



        static void Main(string[] args)
        {
            Reflect.Injector<A>(new List<Type> { typeof(B)});
            var instance = Activator.CreateInstance(typeof(B));
            ConstructorInfo info = typeof(A).GetConstructor(new Type[] { typeof(B) });
            object classAObject = info.Invoke(new object[] { instance });
            /*
            var classB = new B();
            A classA = new A(classB);

            Reflect.ConstructorAboutInfo<A>(typeof(B));
            //    object a = info.Invoke(new object[] { });
            /*
            Initialize(typeof(A));
            MyTestClass mtc = new MyTestClass(12.0, 3.5);
            Reflect.MethodReflectInfo<MyTestClass>(mtc);
            Reflect.FieldInterfaceInfo<MyTestClass>(mtc);

            Console.ReadLine();
            */
        }
    }
}
