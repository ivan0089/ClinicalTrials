using ClinicalTrials.Domain;
using ClinicalTrials.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace ClinicalTrials.Infrastructure.ClinicalTrialContext
{
    public interface IClinicalTrialsDbContext : IDbContext
    {
        public DbSet<ClinicalTrial> ClinicalTrials { get; set; }
    }
}
