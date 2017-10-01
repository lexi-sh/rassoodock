using System.Collections.Generic;

namespace Rassoodock.Databases
{
    public interface IDatabaseReader
    {
        IEnumerable<StoredProcedure> GetStoredProcedures();
    }
}