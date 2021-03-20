using System;
using System.Collections.Generic;
using System.Text;

namespace HW5T1
{
    public class TestInfo
    {
        public string Name { get; private set; }

        public string Result { get; private set; }

        public string WhyIgnored { get; private set; }

        public long TimeOfExecution { get; private set; }

        public TestInfo(string name, string resut, string whyIgnored, long timeOfExecution)
        {
            this.Name = name;
            this.Result = Result;
            this.WhyIgnored = whyIgnored;
            this.TimeOfExecution = timeOfExecution;
        }
    }
}
