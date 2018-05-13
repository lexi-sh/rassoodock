using System;
using Rassoodock.Common;

namespace Rassoodock.Databases.Services
{
    public static class MigrationStrategyFactory
    {
        public static MigrationStrategyBase GetDatabaseGenerationStrategyBase(LinkedDatabase database)
        {
            if (database.DatabaseType == DatabaseType.SqlServer)
            {
                return new SqlServerMigrationStrategy();
            }
            throw new NotImplementedException();
        }
    }
}
