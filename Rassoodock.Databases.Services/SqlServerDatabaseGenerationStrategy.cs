using Rassoodock.Common;
using Rassoodock.SqlServer;

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
