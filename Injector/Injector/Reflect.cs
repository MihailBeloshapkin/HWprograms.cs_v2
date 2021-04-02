using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Injector
{
    class Reflect
    {
        /// <summary>
        /// Search constructor argument realization.
        /// </summary>
        private static Type SearchRealisation(Type arg, List<Type> realisations)
        {
            Type answer = null;
            int countOfRealisations = 0;
            if (arg.IsInterface)
            {
                foreach (var item in realisations)
                {
                    var interfaces = item.GetInterfaces();
                    if (interfaces.Length > 0)
                    {
                        foreach (var currentInterface in interfaces)
                        {
                            if (currentInterface.Name == arg.Name)
                            {
                                countOfRealisations++;
                                answer = item;
                            }
                        }
                    }
                }
            }
            if (arg.IsAbstract)
            {
                foreach (var item in realisations)
                {
                    var baseClasse = item.BaseType;
                    if (baseClasse.Name == arg.Name)
                    {
                        countOfRealisations++;
                        answer = item;
                    }
                }
            }
            if (arg.IsClass)
            {
                foreach (var item in realisations)
                {
                    if (item.IsClass && item.Name == arg.Name)
                    {
                        countOfRealisations++;
                        answer = item;
                    }
                }
                answer = arg;
            }
            if (countOfRealisations > 1)
            {
                throw new Exception($"More than one realization of class {arg.Name}");
            }
            if (countOfRealisations < 1)
            {
                throw new Exception($"No realization for class {arg.Name}");
            }
            return answer;
        }


        /// <summary>
        /// Create instance of an object.
        /// </summary>
        public static Object Injector<T>(List<Type> examples) where T : class
        {
            Type t = typeof(T);
            
            ConstructorInfo[] con = t.GetConstructors();

            var argTypeList = new List<Type>();

            // Get constructor arguments.
            ParameterInfo[] parameters = con[0].GetParameters();
            for (int i = 0; i < parameters.Length; i++)
            {
                argTypeList.Add(parameters[i].ParameterType);
            }
            
            var argTypeArray = new Type[argTypeList.Count];
            argTypeList.CopyTo(argTypeArray);
            List<Object> createdObjects = new List<object>();

            foreach (var arg in argTypeList)
            {
                var instanceType = SearchRealisation(arg, examples);
                var instance = Activator.CreateInstance(instanceType);
                createdObjects.Add(instance);
            }
            ConstructorInfo information = t.GetConstructor(argTypeList.ToArray());
            return information.Invoke(createdObjects.ToArray());
        }
    }
}
