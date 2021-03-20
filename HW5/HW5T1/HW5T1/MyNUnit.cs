using Atributes;
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

        public ConcurrentQueue<ConcurrentQueue<TestInfo>> ClassQueue { get; private set; }

        private readonly string pathForAssemblies;

        public MyNUnit(string pathForAssemblies)
        {
            this.pathForAssemblies = pathForAssemblies;
        }

        public void Execute()
        {
            var files = Directory.EnumerateFiles(this.pathForAssemblies, "*.dll", SearchOption.AllDirectories);
            var assemblies = new ConcurrentQueue<Assembly>();
            Parallel.ForEach(files, x => assemblies.Enqueue(Assembly.LoadFrom(x)));
            var classes = assemblies.Distinct().SelectMany(x => x.ExportedTypes).Where(y => y.IsClass);
            var types = classes.Where(c => c.GetMethods().Any(m => m.GetCustomAttributes().Any(t => t is TestAttribute)));
        }
    }
}
