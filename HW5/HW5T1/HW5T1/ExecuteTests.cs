using Atributes;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;



namespace HW5T1
{
    /// <summary>
    /// This class contains methods for executing tests.
    /// </summary>
    public class ExecuteTests
    {

        private class TestInfo
        {
            public Type ClassName { get; }
            public MethodInfo Method { get; }
            public IEnumerable<MethodInfo> BeforeMethods { get; }
            public IEnumerable<MethodInfo> AfterMethods { get; }
            public TestClassReport ClassReport { get; }

            public TestInfo(Type className, MethodInfo method, IEnumerable<MethodInfo> beforeMethods, IEnumerable<MethodInfo> afterMethods, TestClassReport classReport)
            {
                this.ClassName = className;
                this.Method = method;
                this.BeforeMethods = beforeMethods;
                this.AfterMethods = afterMethods;
                this.ClassReport = classReport;
            }

        }

        /*
        private static void ExecuteTest(TestInfo info)
        {
            var attribute = (TestAttribute)info.Method.GetCustomAttribute(typeof(TestAttribute));

            if (attribute.Ignore != null)
            {
                info.ClassReport.Reports.Add(new SingleTestReport(info.Method.Name, attribute.Ignore));
                return;
            }

            var instance = Activator.CreateInstance(info.ClassName);

            foreach (var method in info.BeforeMethods)
            {
                method.Invoke(instance, null);
            }

            Exception actual = null;
            var stopwatch = new Stopwatch();

            try
            {
                stopwatch.Start();
                info.Method.Invoke(instance, null);
                stopwatch.Stop();
            }
            catch (TargetInvocationException e)
            {
                stopwatch.Stop();
                actual = e.InnerException;
            }
            finally
            {
                info.ClassReport.Reports.Add(new SingleTestReport(info.Method.Name, attribute.Expected, actual, stopwatch.Elapsed));
            }

            foreach (var method in info.AfterMethods)
            {
                method.Invoke(instance, null);
            }
            
        }*/
    }
}
