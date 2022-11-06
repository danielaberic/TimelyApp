using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using timely.Modals;

namespace timely.Data
{
    public class TimelyDbContext : DbContext
    {
        public TimelyDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Project> Timely { get; set; }
        public DbSet<Time> Times { get; set; }
    }
}
