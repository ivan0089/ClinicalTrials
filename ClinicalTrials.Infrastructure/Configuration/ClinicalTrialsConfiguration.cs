using ClinicalTrials.Domain;
using ClinicalTrials.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicalTrials.Infrastructure.Configuration
{
    internal class ClinicalTrialsConfiguration : IEntityTypeConfiguration<ClinicalTrial>
    {
        public void Configure(EntityTypeBuilder<ClinicalTrial> builder)
        {
            builder.HasKey(x => x.Id).IsClustered(false);
            builder.Property(x => x.Id)
                   .IsRequired()
                   .ValueGeneratedNever();
            builder.Property(x => x.TrialId).IsRequired();
            builder.Property(x => x.Title).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.Participants).IsRequired();
            builder.Property(x => x.StartDate).IsRequired();
            builder.Property(x => x.EndDate).IsRequired(false);
            builder.Property(x=> x.DurationInDays).HasDefaultValue(0).IsRequired(false);
            builder.Property<DateTime>("Created")
                  .HasDefaultValueSql(ConfigurationConstants.GetSqlUtcDate);

            builder.HasIndex("Created").IsClustered();
        }
    }
}
