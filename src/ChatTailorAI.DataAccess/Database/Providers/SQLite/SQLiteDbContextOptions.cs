using System.Diagnostics;
using System.Reflection;
using ChatTailorAI.DataAccess.Database.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatTailorAI.DataAccess.Database.Providers.SQLite
{
    public class SQLiteDbContextOptions : IDbContextOptions
    {
        public DbContextOptions Options { get; set; }
        public string ConnectionString { get; private set; }

        public SQLiteDbContextOptions(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public void Configure(DbContextOptionsBuilder optionsBuilder)
        {
            Debug.WriteLine("**** Migrations Assembly: " + Assembly.GetExecutingAssembly().GetName().Name); 

            optionsBuilder.UseSqlite(ConnectionString, options =>
            {
                options.MigrationsAssembly(typeof(Db).Assembly.FullName);
            });

            Options = optionsBuilder.Options;
        }
    }
}