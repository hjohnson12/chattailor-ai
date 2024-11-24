using System.IO;
using ChatTailorAI.DataAccess.Database.Interfaces;

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