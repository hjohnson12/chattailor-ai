using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using ChatTailorAI.DataAccess.Database.Interfaces;
using ChatTailorAI.DataAccess.Database.Providers.SQLite;

namespace ChatTailorAI.Migrations
{
    /// <summary>
    /// Factory class for creating instances of <see cref="SQLiteDb"/> at design time.
    /// </summary>
    public class SQLiteDbContextFactory : IDesignTimeDbContextFactory<SQLiteDb>
    {
        /// <summary>
        /// Creates a new instance of <see cref="SQLiteDb"/> with the specified arguments.
        /// </summary>
        /// <param name="args">The arguments used to create the database context.</param>
        /// <returns>A new instance of <see cref="SQLiteDb"/> configured to use a SQLite database.</returns>
        public SQLiteDb CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseSqlite("Filename=ChatTailorAI.db");

            SQLiteDb db = new(optionsBuilder.Options);
            return db;
        }
    }

    /// <summary>
    /// Provides the path to the SQLite database file.
    /// </summary>
    public class ConsoleDatabasePathProvider : IDatabasePathProvider
    {
        /// <summary>
        /// Gets the path to the SQLite database file.
        /// </summary>
        /// <returns>The path to the SQLite database file.</returns>
        public string GetDatabasePath()
        {
            return "Filename=ChatTailorAI.db";
        }
    }
}