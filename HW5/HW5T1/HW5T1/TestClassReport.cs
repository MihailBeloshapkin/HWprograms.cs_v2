using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace HW5T1
{
    /// <summary>
    /// This class contains some useful information about test class.
    /// </summary>
    public class TestClassReport
    {
        public string NameOfAssembly { get; private set; }
        public string NameOfClass { get; private set; }
        public ConcurrentBag<SingleTestReport> Reports { get; }

        public TestClassReport(string nameOfAssembly, string nameOfClass)
        {
            this.NameOfAssembly = nameOfAssembly;
            this.NameOfClass = nameOfClass;
            this.Reports = new ConcurrentBag<SingleTestReport>();
        }
    }
}
