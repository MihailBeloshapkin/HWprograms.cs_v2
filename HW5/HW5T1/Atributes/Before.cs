using System;

namespace Attributes
{
    /// <summary>
    /// For methods which are executed before each test.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class Before : Attribute
    { }
}
