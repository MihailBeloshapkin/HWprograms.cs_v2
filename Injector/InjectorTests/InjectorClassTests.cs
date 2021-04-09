using NUnit.Framework;
using System;
using System.Reflection;
using System.Collections.Generic;
using Examples;
using Injector;

namespace InjectorTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SimpleTest()
        {
            var objectA = Reflect.Initialize(typeof(A), new List<Type> { typeof(B), typeof(C), typeof(D) });
    
            Assert.AreEqual("A", objectA.GetType().Name);
        }

        [Test]
        public void LoopDependencyTest()
        {
            Assert.Throws<Exception>(() => Reflect.Initialize(typeof(X), new List<Type> { typeof(Y) }));
        }

        [Test]
        public void NoRealizationTest()
        {
            Assert.Throws<Exception>(() => Reflect.Initialize(typeof(A), new List<Type> { typeof(B)}));
        }
    }
}