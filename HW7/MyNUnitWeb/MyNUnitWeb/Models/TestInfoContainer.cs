using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MyNUnitWeb.Models
{
    public class TestInfoContainer
    {
        string Name { get; set; }

        public IList<TestReportModel> TestReports { get; set; } = new List<TestReportModel>();

        [Key]
        public string Id { get; set; }

        public int Passed { get; set; }

        public int Failed { get; set; }

        public int Ignored { get; set; }
    }
}
