using System;
using System.Collections.Generic;
using System.Text;

namespace HW5T1
{
    public class InvalidMethodReport
    {
        public string Name { get; }
        public List<string> Errors { get; }

        public InvalidMethodReport(string name, List<string> error)
        {
            this.Errors = error;
            this.Name = name;
        }
    }
}
