using Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace HW5T1
{
    /// <summary>
    /// Contains static methods to check that declaration is correct.
    /// </summary>
    public static class DeclarationChecker
    {
        /// <summary>
        /// Check that attribute declaration is correct.
        /// </summary>
        public static List<string> CheckAttribute(MethodInfo test, Attribute attribute)
        {
            var attributeName = attribute.GetType().Name;
            var declarationErrors = new List<string>();


            if (attributeName == typeof(AfterClass).Name || attributeName == typeof(BeforeClass).Name)
            {
                if (!test.IsStatic)
                {
                    declarationErrors.Add($"Methods with {attributeName} attribute should be static");
                }
            }

            if (attributeName == typeof(After).Name || attributeName == typeof(Before).Name || attributeName == typeof(Test).Name)
            {
                if (test.ReturnType.Name != "Void")
                {
                    declarationErrors.Add($"Incorrect declaration: {test.Name} should be void");
                }
                if (test.IsStatic)
                {
                    declarationErrors.Add($"Incorrect declaration: {test.Name} shouldn't be static");
                }
                if (test.GetParameters().Length != 0)
                {
                    declarationErrors.Add($"Incorrect declaration: {test.Name} shouldn't have input parameters");
                }
            }
            return declarationErrors;
        }
    }
}
