using ChatTailorAI.DataAccess.Database.Providers.SQLite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.DataAccess.Database
{
    public static class DbContextConfiguration
    {
        /// <summary>
        /// Configures the DbContextOptionsBuilder based on the database type in the configuration.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <param name="configuration"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void ConfigureDbContextOptions(DbContextOptionsBuilder optionsBuilder, IConfiguration configuration)
        {
            var dbType = configuration["DatabaseType"];
            switch (dbType)
            {
                case "SQLite":
                    var sqliteConnStr = configuration.GetConnectionString("SQLite");
                    var sqliteOptions = new SQLiteDbContextOptions(sqliteConnStr);
                    sqliteOptions.Configure(optionsBuilder);
                    break;
                default:
                    throw new InvalidOperationException("Unsupported database type");
            }
        }
    }
}