using System;
using System.Collections.Generic;
using Rassoodock.Databases;

namespace Rassoodock.SqlServer.Windows
{
    public class DatabaseReader : IDatabaseReader
    {
        public IEnumerable<StoredProcedure> GetStoredProcedures()
        {
            throw new NotImplementedException();
        }
    }
}
