using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Injector
{
    public class Reflect
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
        public static Object Initialize(Type root, List<Type> examples)
        {
            return localInitialize(root, examples, null);
        }

        /// <summary>
        /// Create instance of an object. Recursive initialization.
        /// </summary>
        private static Object localInitialize(Type root, List<Type> examples, List<Type> prevDependencies)
        {
            ConstructorInfo[] con = root.GetConstructors();

            var argTypeList = new List<Type>();

            if (prevDependencies == null)
            {
                prevDependencies = new List<Type>();
            }
            // Get constructor arguments.
            ParameterInfo[] parameters = con[0].GetParameters();
            for (int i = 0; i < parameters.Length; i++)
            {
                if (prevDependencies.Contains(parameters[i].ParameterType))
                {
                    throw new Exception("Dependency loop.");
                }
                argTypeList.Add(parameters[i].ParameterType);
            }

            var argTypeArray = new Type[argTypeList.Count];
            argTypeList.CopyTo(argTypeArray);
            List<Object> createdObjects = new List<object>();

            foreach (var arg in argTypeList)
            {
                var instanceType = SearchRealisation(arg, examples);
                examples.Remove(instanceType);
                prevDependencies.Add(instanceType);
                var instance = localInitialize(instanceType, examples, prevDependencies);
                createdObjects.Add(instance);
            }
            ConstructorInfo information = root.GetConstructor(argTypeList.ToArray());
            return information.Invoke(createdObjects.ToArray());
        }
    }
}
