
using ClinicalTrials.Domain;
using ClinicalTrials.Infrastructure.ClinicalTrialContext;
using Microsoft.EntityFrameworkCore;

namespace ClinicalTrials.Infrastructure.Database
{
    public class DatabaseContext : DbContext, IClinicalTrialsDbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<ClinicalTrial> ClinicalTrials { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schemas.Default);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
