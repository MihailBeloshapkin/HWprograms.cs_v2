using Attributes;
using System;
using System.Collections.Generic;
using System.Text;


namespace HW5T1
{
    /// <summary>
    /// Sort methods to the set of lists in SetOfMethods class.
    /// </summary>
    public static class Distribution
    {
        public static List<string> SortMethods(out SetOfMethods methods, Type type)
        {
            methods = new SetOfMethods();
            var declarationErrors = new List<string>();

            foreach (var method in type.GetMethods())
            {
                foreach (var attribute in Attribute.GetCustomAttributes(method))
                {
                    declarationErrors = DeclarationChecker.CheckAttribute(method, attribute);
                    if (declarationErrors.Count != 0)
                    {
                        return declarationErrors;
                    }


                    if (attribute.GetType() == typeof(Before))
                    {
                        methods.Before.Add(method);
                    }
                    if (attribute.GetType() == typeof(After))
                    {
                        methods.After.Add(method);
                    }
                    if (attribute.GetType() == typeof(BeforeClass))
                    {
                        methods.BeforeClass.Add(method);
                    }
                    if (attribute.GetType() == typeof(AfterClass))
                    {
                        methods.AfterClass.Add(method);
                    }
                    if (attribute.GetType() == typeof(Test))
                    {
                        methods.Tests.Add(method);
                    }
                }
            }

            return null;
        }
    }
}
    