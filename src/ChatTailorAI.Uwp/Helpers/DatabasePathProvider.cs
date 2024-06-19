using ChatTailorAI.DataAccess.Database.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatTailorAI.Uwp.Helpers
{
    public class DatabasePathProvider : IDatabasePathProvider
    {
        public string GetDatabasePath()
        {
            return Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "ChatTailorAI.db");
        }
    }
}
