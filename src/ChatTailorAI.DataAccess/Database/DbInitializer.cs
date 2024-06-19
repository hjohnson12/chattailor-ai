using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ChatTailorAI.DataAccess.Database.Providers.SQLite;

namespace ChatTailorAI.DataAccess.Database
{
    public class DbInitializer
    {
        private readonly SQLiteDb _dbContext;

        public DbInitializer(SQLiteDb dbContext)
        {
           _dbContext = dbContext;
        }

        public async Task InitializeAsync()
        {
            var pendingMigrations = (await _dbContext.Database.GetPendingMigrationsAsync()).ToList();
            var applied = (await _dbContext.Database.GetAppliedMigrationsAsync()).ToList();
            var other = _dbContext.Database.GetMigrations().ToList();
            var connectionString = _dbContext.Database.GetDbConnection().ConnectionString;
            var providerName = _dbContext.Database.ProviderName;

            Console.WriteLine($"Migrations: {string.Join(", ", pendingMigrations)}");
            Console.WriteLine($"Applied: {string.Join(", ", applied)}");
            Console.WriteLine($"Other: {string.Join(", ", other)}");
            Console.WriteLine($"Connection: {connectionString}");
            Console.WriteLine($"Provider: {providerName}");

            if (pendingMigrations.Any())
            {
                await _dbContext.Database.MigrateAsync();
            }
        }
    }
}