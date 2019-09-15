using Microsoft.EntityFrameworkCore;
using Model;

namespace StepChallenge
{
    public class StepContext : DbContext
    {
        public DbSet<Team> Team { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Steps> Steps { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=StepChallenge.db");
        }
        
    }
}