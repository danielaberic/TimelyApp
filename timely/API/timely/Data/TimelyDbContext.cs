using Microsoft.EntityFrameworkCore;
using timely.Models;

namespace timely.Data
{
    public class TimelyDbContext:DbContext
    {
        public TimelyDbContext(DbContextOptions options):base(options)
        {
        }
        public DbSet<Project> Timely { get; set; }
    }
}
