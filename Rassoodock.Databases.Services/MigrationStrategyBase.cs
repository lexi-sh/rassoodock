using Rassoodock.Common;

namespace Rassoodock.Databases.Services
{
    public abstract class MigrationStrategyBase
    {
        public abstract string GetMigrationScript(LinkedDatabase database);
    }
}