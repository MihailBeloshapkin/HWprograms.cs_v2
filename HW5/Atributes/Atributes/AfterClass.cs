using System;

namespace Atributes
{
    /// <summary>
    /// For method which is executed after single test
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class AfterAttribute : Attribute
    { }
}
