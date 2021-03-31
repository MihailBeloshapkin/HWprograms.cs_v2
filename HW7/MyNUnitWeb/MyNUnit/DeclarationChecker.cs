using Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace HW5T1
{
    public static class DeclarationChecker
    {
        /// <summary>
        /// Check that attribute declaration is correct.
        /// </summary>
        public static void CheckAttribute(MethodInfo test, Attribute attribute, out List<string> declarationErrors)
        {
            var attributeName = attribute.GetType().Name;
            declarationErrors = new List<string>();

            if (attributeName == typeof(AfterClass).Name || attributeName == typeof(BeforeClass).Name)
            {
                if (!test.IsStatic)
                {
                    declarationErrors.Add($"Methods with {attributeName} attribute should be static");
                    return;
                }
            }

            if (attributeName == typeof(After).Name || attributeName == typeof(Before).Name || attributeName == typeof(Test).Name)
            {
                if (test.ReturnType.Name != "Void")
                {
                    declarationErrors.Add($"Incorrect declaration: {test.Name} should be void");
                    return;
                }
                if (test.IsStatic)
                {
                    declarationErrors.Add($"Incorrect declaration: {test.Name} should be static");
                    return;
                }
            }
        }
    }
}