using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.DataAccess.Database.Providers.SQLite
{
    public class SQLiteDb : Db
    {
        public SQLiteDb(DbContextOptions options) : base(options)
        {
        }
    }
}