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
    }
}
