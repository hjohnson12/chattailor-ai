using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.DataAccess.Database.Interfaces
{
    public interface IDbContextOptions
    {
        DbContextOptions Options { get; }
    }
}