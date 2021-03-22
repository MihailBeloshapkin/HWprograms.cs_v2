using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace HW5T1
{
    // This class contains the set of methods.
    public class SetOfMethods
    {
        /// <summary>
        /// Before List.
        /// </summary>
        public List<MethodInfo> Before { get; set; } = new List<MethodInfo>();

        /// <summary>
        /// After List.
        /// </summary>
        public List<MethodInfo> After { get; set; } = new List<MethodInfo>();

        /// <summary>
        /// Tests List.
        /// </summary>
        public List<MethodInfo> Tests { get; set; } = new List<MethodInfo>();

        /// <summary>
        /// BeforeClass List.
        /// </summary>
        public List<MethodInfo> BeforeClass { get; set; } = new List<MethodInfo>();

        /// <summary>
        /// AfterClass List.
        /// </summary>
        public List<MethodInfo> AfterClass { get; set; } = new List<MethodInfo>();
    }
}
