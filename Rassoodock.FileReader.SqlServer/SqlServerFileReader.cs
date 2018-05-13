using Rassoodock.SqlServer.Models.Code;
using System.Collections.Generic;
using System.Linq;

namespace Rassoodock.FileReader.SqlServer
{
    public class SqlServerFileReader
    {
        private readonly SqlServerStoredProceduresFileReader _storedProcedureFileReader;

        public SqlServerFileReader()
        {
            _storedProcedureFileReader = new SqlServerStoredProceduresFileReader();
        }

        public IEnumerable<SqlServerStoredProcedure> StoredProcedures(IEnumerable<string> filePaths)
        {
            return filePaths.Select(_storedProcedureFileReader.ReadStoredProcedure);
        }
    }
}
