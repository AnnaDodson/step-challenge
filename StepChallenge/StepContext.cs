using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Model;

namespace StepChallenge
{
    public partial class StepContext : IdentityDbContext<IdentityUser >
    {
        public virtual DbSet<Team> Team { get; set; }
        public virtual DbSet<Participant> Participants { get; set; }
        public virtual DbSet<Steps> Steps { get; set; }
        public virtual DbSet<ChallengeSettings> ChallengeSettings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=db/StepChallenge.db");
        }
        
    }
}
