using System;

namespace Attributes
{
    /// <summary>
    /// For method which is executed after all test run.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class AfterClass : Attribute
    { }
}
