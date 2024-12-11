
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;

namespace ClinicalTrials.Infrastructure.Database
{
    public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            const string rootDirectoryName = @"ClinicalTrials";
            const string apiProjectName = @"ClinicalTrials.Api";
            var index = Directory.GetCurrentDirectory().IndexOf(rootDirectoryName, StringComparison.Ordinal);
            var rootPath = Directory.GetCurrentDirectory().Substring(0, index + rootDirectoryName.Length);

            var basePath = Path.Combine(rootPath, apiProjectName);

            if (!Directory.Exists(basePath))
            {
                var directories = Directory.GetDirectories(Directory.GetCurrentDirectory().Substring(0, index), apiProjectName, SearchOption.AllDirectories);
                basePath = directories.First();
            }

            var configuration = new ConfigurationBuilder()
         .SetBasePath(basePath) // Set the base path to the current directory
         .AddJsonFile("appsettings.json") 
         .Build();

            // Get the connection string
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer(connectionString,
                                        options =>
                                        {
                                            options.MigrationsAssembly(GetType().Assembly.FullName);
                                            options.MigrationsHistoryTable(HistoryRepository.DefaultTableName,
                                                                           Schemas.Default);
                                        });
            return new DatabaseContext(optionsBuilder.Options);
        }
    }
}
