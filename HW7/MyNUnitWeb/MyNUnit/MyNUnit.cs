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
        private List<string> declarationErrors;

        private readonly string pathToAssemblies;

        private SetOfMethods methods;

        public ConcurrentQueue<ConcurrentQueue<TestData>> GeneralSet { get; private set; }


        public MyNUnit(string pathToAssemblies)
        {
            this.pathToAssemblies = pathToAssemblies;
            this.declarationErrors = new List<string>();
        }

        /// <summary>
        /// Get list of methods and delete all data from queue.
        /// </summary>
        public List<TestData> GetAllData()
        {
            var allData = new List<TestData>();
            while (!this.GeneralSet.IsEmpty)
            {
                this.GeneralSet.TryDequeue(out var information);
                while (!information.IsEmpty)
                {
                    information.TryDequeue(out var data);
                    allData.Add(data);
                }
            }

            return allData;
        }

        /// <summary>
        /// Load all data and execute tests from specified path.
        /// </summary>
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
            var types = classes.Where(x => x.GetMethods().Any(y => y.GetCustomAttributes().Any(z => z is Test)));
            this.GeneralSet = new ConcurrentQueue<ConcurrentQueue<TestData>>();

            Parallel.ForEach(types, (type) =>
            {
                this.declarationErrors = Distribution.SortMethods(out methods, type);
                if (this.declarationErrors != null && this.declarationErrors.Count != 0)
                {
                    return;
                }

                var testInfo = new ConcurrentQueue<TestData>();

                if (!this.TryToExecuteBeforeClassOrAfterClassTest(testInfo, this.methods.BeforeClass))
                {
                    this.GeneralSet.Enqueue(testInfo);
                    return;
                }

                var currentQueue = new ConcurrentQueue<TestData>();
                Parallel.ForEach(this.methods.Tests, (test) => this.ExecuteTest(type, test, currentQueue));

                if (!this.TryToExecuteBeforeClassOrAfterClassTest(testInfo, this.methods.AfterClass))
                {
                    this.GeneralSet.Enqueue(testInfo);
                    return;
                }
                this.GeneralSet.Enqueue(currentQueue);
            });
        }

        /// <summary>
        /// Execute test. 
        /// </summary>
        private void ExecuteTest(Type type, MethodInfo method, ConcurrentQueue<TestData> queue)
        {
            var property = (Test)Attribute.GetCustomAttribute(method, typeof(Test));
            var preName = type.Assembly.FullName.Split(' ')[0].TrimEnd(',') + ',' + type.Name + ',';

            if (property.Ignore != null)
            {
                queue.Enqueue(new TestData(preName + method.Name, "Ignored", property.Ignore, 0));
                return;
            }

            var instance = Activator.CreateInstance(type);

            var exceptionBefore = this.TryToExecuteAfterOrBeforeTest(instance, this.methods.Before);
            if (exceptionBefore != null)
            {
                queue.Enqueue(new TestData(preName + method.Name, "Failed", exceptionBefore, 0));
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
                    queue.Enqueue(new TestData(preName + method.Name, "Success", e.Message, stopWatch.ElapsedMilliseconds));
                }
                else
                {
                    queue.Enqueue(new TestData(preName + method.Name, "Failed", e.Message, stopWatch.ElapsedMilliseconds));
                }

                return;
            }

            if (property.Expected != null)
            {
                queue.Enqueue(new TestData(preName + method.Name, "Failed",
                    $"Test did not throw an exception: {property.Expected.ToString()}", stopWatch.ElapsedMilliseconds));
                return;
            }

            var exceptionAfter = this.TryToExecuteAfterOrBeforeTest(instance, methods.After);
            if (exceptionAfter != null)
            {
                queue.Enqueue(new TestData(preName + method.Name, "Failed", exceptionAfter, 0));
                return;
            }

            queue.Enqueue(new TestData(preName + method.Name, "Success", null, stopWatch.ElapsedMilliseconds));
        }

        /// <summary>
        /// Display result of test execution to a console.
        /// </summary>
        public void DisplayResults()
        {
            if (this.declarationErrors != null && this.declarationErrors.Count != 0)
            {
                Console.WriteLine("ERRORS:");
                foreach (var error in this.declarationErrors)
                {
                    Console.WriteLine(error);
                }
            }

            while (!this.GeneralSet.IsEmpty)
            {
                this.GeneralSet.TryDequeue(out var information);
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

        private string TryToExecuteAfterOrBeforeTest(object instance, List<MethodInfo> methods)
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

        private bool TryToExecuteBeforeClassOrAfterClassTest(ConcurrentQueue<TestData> testInfo, List<MethodInfo> methods)
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
                        testInfo.Enqueue(new TestData(test.Name, "Failed", e.Message, 0));
                    }

                    return false;
                }
            }

            return true;
        }
    }
}