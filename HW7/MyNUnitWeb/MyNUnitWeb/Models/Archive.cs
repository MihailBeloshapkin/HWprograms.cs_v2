using Microsoft.EntityFrameworkCore;

namespace MyNUnitWeb.Models
{
    public class Archive : DbContext
    {
        public Archive(DbContextOptions<Archive> options)
                : base(options)
        {
        }

        public DbSet<TestReportModel> reports { get; set; }
    }
}

