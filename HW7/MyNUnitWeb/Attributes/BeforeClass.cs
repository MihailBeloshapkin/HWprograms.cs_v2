using System;

namespace Attributes
{
    /// <summary>
    /// For method which is executed before single test
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class BeforeClass : Attribute
    { }
}