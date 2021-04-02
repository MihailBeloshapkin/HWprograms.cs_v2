using System;

namespace Attributes
{
    /// <summary>
    /// For methods which are executed after each test.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class After : Attribute
    { }
}
