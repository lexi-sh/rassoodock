using System;
using Rassoodock.Common;

namespace Rassoodock.Databases.Services
{
    public static class DatabaseGenerationStrategyFactory
    {
        public static DatabaseGenerationStrategyBase GetDatabaseGenerationStrategyBase(LinkedDatabase database)
        {
            if (database.DatabaseType == DatabaseType.SqlServer)
            {
                return new SqlServerDatabaseGenerationStrategy();
            }
            throw new NotImplementedException();
        }
    }
}
