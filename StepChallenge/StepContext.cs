using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Model;

namespace StepChallenge
{
    public partial class StepContext : IdentityDbContext<IdentityUser >
    {
        public DbSet<Team> Team { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Steps> Steps { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=db/StepChallenge.db");
        }
        
    }
}
