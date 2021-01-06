using System;

namespace Atributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class Before : Attribute
    { }
}
