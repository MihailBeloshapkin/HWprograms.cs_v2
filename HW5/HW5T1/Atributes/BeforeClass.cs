using System;

namespace Attributes
{
    /// <summary>
    /// For method which is executed before all test run.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class BeforeClass : Attribute
    { }
}
