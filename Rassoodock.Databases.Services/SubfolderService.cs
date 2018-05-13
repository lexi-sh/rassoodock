using System.IO;
using Rassoodock.Common;

namespace Rassoodock.Databases.Services
{
    public static class SubfolderService
    {
        public static string GetStoredProceduresFolder(LinkedDatabase database)
        {

            var directory = Directory.GetCurrentDirectory();
            var folderName = Path.Combine(directory, database.Name);
            var storedProcFolderName = Path.Combine(folderName, "Stored Procedures");
            if (!Directory.Exists(storedProcFolderName))
            {
                Directory.CreateDirectory(storedProcFolderName);
            }
            return storedProcFolderName;
        }

        public static string GetTableFolder(LinkedDatabase database)
        {
            var directory = Directory.GetCurrentDirectory();
            var folderName = Path.Combine(directory, database.Name);
            var tableFolderName = Path.Combine(folderName, "Tables");
            if (!Directory.Exists(tableFolderName))
            {
                Directory.CreateDirectory(tableFolderName);
            }
            return tableFolderName;
        }
    }
}
