using Rassoodock.Common;
using Rassoodock.SqlServer;

namespace Rassoodock.Databases.Services
{
    public class SqlServerDatabaseGenerationStrategy : DatabaseGenerationStrategyBase
    {
        public override Database GetDatabase(LinkedDatabase database)
        {
            var sqlServerRepository = new SqlServerDatabaseReader(database);
            return new Database
            {
                StoredProcedures = sqlServerRepository.GetStoredProcedures(),
                Tables = sqlServerRepository.GetTables()
            };
        }
    }
}
