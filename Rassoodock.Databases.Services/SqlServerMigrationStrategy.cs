using Rassoodock.Common;
using Rassoodock.DifferenceEngine.SqlServer;
using Rassoodock.FileReader.SqlServer;
using Rassoodock.SqlServer;
using System.IO;

namespace Rassoodock.Databases.Services
{
    public class SqlServerMigrationStrategy : MigrationStrategyBase
    {
        public override string GetMigrationScript(LinkedDatabase database)
        {
            var sqlServerStoredProcReader = new StoredProcedureDatabaseReader(database);
            var fileReader = new SqlServerFileReader();

            var storedProcFilePaths = Directory.GetFiles(SubfolderService.GetStoredProceduresFolder(database), ".sql");

            var storedProcDifferentiator = new SqlServerStoredProcedureDifferentiator();
            return storedProcDifferentiator.GetDifferenceAlterString(fileReader.StoredProcedures(storedProcFilePaths), sqlServerStoredProcReader.GetStoredProcedures());
        }
    }
}
