using Rassoodock.Common;

namespace Rassoodock.Databases.Services
{
    public abstract class DatabaseGenerationStrategyBase
    {
        public abstract Database GetDatabase(LinkedDatabase database);
    }
}
