using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class CandidateDbContext : DbContext
    {
        public CandidateDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CandidateConfig());
        }

        public DbSet<Candidate> Candidates => Set<Candidate>();
    }
}