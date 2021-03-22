using Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;



namespace HW5T1
{
    /// <summary>
    /// This class contains methods for executing tests.
    /// </summary>
    public class MyNUnit
    {
        private SetOfMethods methods;

        private ConcurrentQueue<ConcurrentQueue<TestInfo>> ClassQueue;

        private readonly string pathToAssemblies;

        public MyNUnit(string pathToAssemblies)
        {
            this.pathToAssemblies = pathToAssemblies;
        }

        public void Execute()
        {
            var files = Directory.EnumerateFiles(this.pathToAssemblies, "*.dll", SearchOption.AllDirectories);
            if (files.Count() == 0)
            {
                throw new Exception("Assemblies not found");
            }

            var assemblies = new ConcurrentQueue<Assembly>();
            Parallel.ForEach(files, x => assemblies.Enqueue(Assembly.LoadFrom(x)));
            var classes = assemblies.Distinct().SelectMany(x => x.ExportedTypes).Where(y => y.IsClass);
            var types = classes.Where(c => c.GetMethods().Any(m => m.GetCustomAttributes().Any(t => t is Test)));
            this.ClassQueue = new ConcurrentQueue<ConcurrentQueue<TestInfo>>();
            
            Parallel.ForEach(types, (type) => 
            {
                DistributeMethodsByAttributes(type);
                var testInfo = new ConcurrentQueue<TestInfo>();

                if (!this.BeforeClassOrAfterClassTest(testInfo, this.methods.BeforeClass))
                {
                    this.ClassQueue.Enqueue(testInfo);
                    return;
                }

                var currentQueue = new ConcurrentQueue<TestInfo>();
                Parallel.ForEach(this.methods.Tests, (test) => this.RunTest(type, test, currentQueue));

                if (!this.BeforeClassOrAfterClassTest(testInfo, this.methods.AfterClass))
                {
                    this.ClassQueue.Enqueue(testInfo);
                    return;
                }

                this.ClassQueue.Enqueue(currentQueue);
            });

            this.DisplayResults();
        }

        /// <summary>
        /// Execute test. 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="method"></param>
        /// <param name="queue"></param>
        private void RunTest(Type type, MethodInfo method, ConcurrentQueue<TestInfo> queue)
        {
            var property = (Test)Attribute.GetCustomAttribute(method, typeof(Test));
            if (property.Ignore != null)
            {
                queue.Enqueue(new TestInfo(method.Name, "Ignored", property.Ignore, 0));
                return;
            }

            var instance = Activator.CreateInstance(type);

            var exceptionBefore = this.AfterOrBeforeTest(instance, this.methods.Before);
            if (exceptionBefore != null)
            {
                queue.Enqueue(new TestInfo(method.Name, "Failed", exceptionBefore, 0));
                return;
            }

            var stopWatch = new Stopwatch();

            try
            {
                stopWatch.Start();
                method.Invoke(instance, null);
                stopWatch.Stop();
            }
            catch (Exception e)
            {
                stopWatch.Stop();
                if (e.InnerException.GetType() == property.Expected)
                {
                    queue.Enqueue(new TestInfo(method.Name, "Success", e.Message, stopWatch.ElapsedMilliseconds));
                }
                else
                {
                    queue.Enqueue(new TestInfo(method.Name, "Failed", e.Message, stopWatch.ElapsedMilliseconds));
                }

                return;
            }

            if (property.Expected != null)
            {
                queue.Enqueue(new TestInfo(method.Name, "Failed", 
                    $"Test did not throw an exception: {property.Expected.ToString()}", stopWatch.ElapsedMilliseconds));
                return;
            }

            var exceptionAfter = this.AfterOrBeforeTest(instance, methods.After);
            if (exceptionAfter != null)
            {
                queue.Enqueue(new TestInfo(method.Name, "Failed", exceptionAfter, 0));
                return;
            }

            queue.Enqueue(new TestInfo(method.Name, "Success", null, stopWatch.ElapsedMilliseconds));
        }

        private void DistributeMethodsByAttributes(Type type)
        {
            methods = new SetOfMethods();

            foreach (var method in type.GetMethods())
            {
                foreach (var attribute in Attribute.GetCustomAttributes(method))
                {
                    this.ValidationOfTestForCorrectness(method, attribute);

                    if (attribute.GetType() == typeof(Before))
                    {
                        this.methods.Before.Add(method);
                    }
                    if (attribute.GetType() == typeof(After))
                    {
                        this.methods.After.Add(method);
                    }
                    if (attribute.GetType() == typeof(BeforeClass))
                    {
                        this.methods.BeforeClass.Add(method);
                    }
                    if (attribute.GetType() == typeof(AfterClass))
                    {
                        this.methods.AfterClass.Add(method);
                    }
                    if (attribute.GetType() == typeof(Test))
                    {
                        this.methods.Tests.Add(method);
                    }
                }
            }
        }

        private void ValidationOfTestForCorrectness(MethodInfo test, Attribute attribute)
        {
            var attributeName = attribute.GetType().Name;

            if (attributeName == typeof(After).Name || attributeName == typeof(Before).Name || attributeName == typeof(Test).Name)
            {
                if (test.ReturnType.Name != "Void" || test.GetParameters().Length > 0 || test.IsStatic)
                {
                    throw new Exception($"Incorrect declaration of {test.Name}");
                }
            }

            if (attributeName == typeof(AfterClass).Name || attributeName == typeof(BeforeClass).Name)
            {
                if (!test.IsStatic)
                {
                    throw new Exception("Methods with this attributes should be static");
                }
            }
        }

        private string AfterOrBeforeTest(object instance, List<MethodInfo> methods)
        {
            foreach (var method in methods)
            {
                try
                {
                    method.Invoke(instance, null);
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }
            return null;
        }

        private bool BeforeClassOrAfterClassTest(ConcurrentQueue<TestInfo> testInfo, List<MethodInfo> methods)
        {
            foreach (var method in methods)
            {
                try
                {
                    method.Invoke(null, null);
                }
                catch (Exception e)
                {
                    foreach (var test in this.methods.Tests)
                    {
                        testInfo.Enqueue(new TestInfo(test.Name, "failed", e.Message, 0));
                    }

                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Display result of test execution to a console.
        /// </summary>
        private void DisplayResults()
        {
            while (!this.ClassQueue.IsEmpty)
            {
                this.ClassQueue.TryDequeue(out var information);
                var result = information.ToArray();
                foreach (var item in result)
                {
                    Console.WriteLine($"Test name: {item.Name}");
                    Console.WriteLine($"Result: {item.Result}");
                    Console.Write("Ignore reason:");
                    if (item.WhyIgnored != null)
                    {
                        Console.Write($"{item.WhyIgnored}");
                    }
                    else
                    {
                        Console.Write("None");
                    }
                    Console.WriteLine();
                    Console.WriteLine($"Time of execution: {item.TimeOfExecution}");
                }
            }
        }
    }
}
