using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Injector
{
    class Reflect
    {
        // Информация о полях и реализуемых интерфейсах 
        public static void FieldInterfaceInfo<T>(T obj) where T : class
        {
            Type t = typeof(T);
            Console.WriteLine("\n*** Реализуемые интерфейсы ***\n");
            var im = t.GetInterfaces();
            foreach (Type tp in im)
                Console.WriteLine("--> " + tp.Name);
            Console.WriteLine("\n*** Поля и свойства ***\n");
            FieldInfo[] fieldNames = t.GetFields();
            foreach (FieldInfo fil in fieldNames)
                Console.Write("--> " + fil.ReflectedType.Name + " " + fil.Name + "\n");
        }

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
            return answer;
            return null;
        }


        public static Object Injector<T>(List<Type> examples) where T : class
        {
            Type t = typeof(T);
         //   t.GetInterfaces();
            
            ConstructorInfo[] con = t.GetConstructors();

            var argTypeList = new List<Type>();
            
            Object[] objectArray = new Object[examples.Count];



        //    examples.CopyTo(new Type[] { });
            bool[] isActivated = new bool[] { false };

            // Get constructor arguments.
            foreach (ConstructorInfo info in con)
            {
                ParameterInfo[] parameters = info.GetParameters();
                for (int i = 0; i < parameters.Length; i++)
                {
                    argTypeList.Add(parameters[i].ParameterType);
                }
            }

            var argTypeArray = new Type[argTypeList.Count];
            argTypeList.CopyTo(argTypeArray);
            List<Object> createdObjects = new List<object>();

            int iter = 0;
            foreach (var arg in argTypeList)
            {
                /*
                if (!examples.Contains(arg))
                {
                    throw new Exception("No dependency realization!");
                }
                */
                var instanceType = SearchRealisation(arg, examples);
                var instance = Activator.CreateInstance(instanceType);
                createdObjects.Add(instance);
                objectArray[iter] = instance;
            }
            ConstructorInfo information = t.GetConstructor(argTypeArray);
            return information.Invoke(objectArray);
        }

        // Данный метод выводит информацию о содержащихся в классе методах
        public static void MethodReflectInfo<T>(T obj) where T : class
        {
            Type t = typeof(T);
            // Получаем коллекцию методов
            MethodInfo[] MArr = t.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
            Console.WriteLine("*** Список методов класса {0} ***\n", obj.ToString());

            // Вывести методы
            foreach (MethodInfo m in MArr)
            {
                Console.Write(" --> " + m.ReturnType.Name + " \t" + m.Name + "(");
                // Вывести параметры методов
                ParameterInfo[] p = m.GetParameters();
                for (int i = 0; i < p.Length; i++)
                {
                    Console.Write(p[i].ParameterType.Name + " " + p[i].Name);
                    if (i + 1 < p.Length) Console.Write(", ");
                }
                Console.Write(")\n");
            }
        }
    }
}
