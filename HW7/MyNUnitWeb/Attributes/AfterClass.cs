using System;

namespace Attributes
{
    /// <summary>
    /// For method which is executed after single test
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class AfterClass : Attribute
    { }
}