using System;
using System.Collections.Generic;
using System.Text;

namespace HW5T1
{
    public enum Result
    { 
        Success,
        Failed,
        Ignored
    }

    public class TestData
    {
        /// <summary>
        /// Name of test.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Result of test.
        /// </summary>
        public Result Result { get; private set; }

        /// <summary>
        /// The reason why test wass ignored.
        /// </summary>
        public string WhyIgnored { get; private set; }

        /// <summary>
        /// Time of test execution in milliseconds.
        /// </summary>
        public long TimeOfExecution { get; private set; }

        public TestData(string name, Result result, string whyIgnored, long timeOfExecution)
        {
            this.Name = name;
            this.Result = result;
            this.WhyIgnored = whyIgnored;
            this.TimeOfExecution = timeOfExecution;
        }
    }
}
