using Rassoodock.Common;
using Rassoodock.SqlServer.Windows;

namespace Rassoodock.Databases.Services
{
    public class SqlServerDatabaseGenerationStrategy : DatabaseGenerationStrategyBase
    {
        public override Database GetDatabase(LinkedDatabase database)
        {
            var sqlServerRepository = new DatabaseReader(database);
            return new Database
            {
                StoredProcedures = sqlServerRepository.GetStoredProcedures()
            };
        }
    }
}
