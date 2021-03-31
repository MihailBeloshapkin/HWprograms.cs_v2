using System;
using System.ComponentModel.DataAnnotations;

namespace MyNUnitWeb.Models
{
    public class TestReportModel
    {
        public string ClassName { get; set; }

        /// <summary>
        /// Name of the method
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// True if test is valid else false
        /// </summary>
        public bool Valid { get; set; }

        /// <summary>
        /// Null if test was ignored, false if failed, true if passed
        /// </summary>
        public bool? Passed { get; set; }

        /// <summary>
        /// Time of the test execution, null if test was ignored
        /// </summary>
        public TimeSpan? Time { get; set; }

        /// <summary>
        /// Reason of ignore or failure of the test
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Unique value to be key in db
        /// </summary>
        [Key]
        public string Id { get; set; }

    }
}

