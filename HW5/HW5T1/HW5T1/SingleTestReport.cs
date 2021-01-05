using System;
using System.Collections.Generic;
using System.Text;

namespace HW5T1
{
    public class SingleTestReport
    {
        public string Name { get; }
        public TimeSpan Time { get; }
        public Exception Actual { get; }
        public Type Expected { get; }
        public string IgnoreCause { get; }

        public bool Ignored 
        {
            get
            {
                return this.IgnoreCause != null;
            }
        }

        public SingleTestReport(string name, Type expected, Exception actual, TimeSpan time)
        {
            this.Name = name;
            this.Expected = expected;
            this.Actual = actual;
            this.Time = time;
        }

        public string Message
        {
            get
            {
                if (Ignored)
                {
                    return IgnoreCause;
                }
                if (Expected != null && Actual == null)
                {
                    return $"expected {Expected} but was null";
                }
                else if (Expected == null && Actual != null)
                {
                    return $"unexpected {Actual.GetType()}";
                }
                else if (Expected != null && Actual != null)
                {
                    return $"expected {Expected} but was {Actual.GetType()}";
                }
                return "";
            }
        }
    }
}
