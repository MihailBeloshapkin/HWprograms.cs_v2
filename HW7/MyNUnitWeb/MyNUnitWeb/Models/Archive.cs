using Microsoft.EntityFrameworkCore;

namespace MyNUnitWeb.Models
{
    public class Archive : DbContext
    {
        public DbSet<TestReportModel> reports { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Archive;Trusted_Connection=True;");
    }
}

