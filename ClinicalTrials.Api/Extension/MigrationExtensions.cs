using ClinicalTrials.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace ClinicalTrials.Api.Extension
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using DatabaseContext dbContext =
                scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            var pendingMigrations = dbContext.Database.GetPendingMigrations();

            if (pendingMigrations.Any())
            {
                dbContext.Database.Migrate();
            }
        }
    }
}
