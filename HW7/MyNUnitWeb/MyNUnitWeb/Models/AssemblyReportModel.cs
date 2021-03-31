using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MyNUnitWeb.Models
{
    public class AssemblyReportModel
    {
        public string Name { get; set; }

        public List<TestReportModel> TestReports { get; set; } = new List<TestReportModel>();

        public bool Valid { get => !TestReports.Any(r => !r.Valid); } 
        
        [Key]
        public string Id { get; set; }

        public int Passed { get; set; }

        public int Failed { get; set; }

        public int Ignored { get; set; }
    }
}
