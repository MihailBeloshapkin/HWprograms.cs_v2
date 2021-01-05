using System;
using System.Collections.Generic;
using System.Text;

namespace HW5T1
{
    public class InvalidTestClassReport
    {
        public string Name { get; }

        public ICollection<InvalidMethodReport> InvalidMethods { get; }

        public InvalidTestClassReport(string name, ICollection<InvalidMethodReport> invalidMethods)
        {
            this.Name = name;
            this.InvalidMethods = invalidMethods;
        }
    }
}
