using System;
using System.Collections.Generic;
using Rassoodock.SqlServer.Models.Domain;

namespace Rassoodock.DifferenceEngine.SqlServer
{
    public class SqlServerTableDifferentiator : IDifferentiator<SqlServerTable>
    {
        public string GetDifferenceAlterString(IEnumerable<SqlServerTable> objectsFromFileSystem, IEnumerable<SqlServerTable> objectsInDb)
        {
            throw new NotImplementedException();
        }
    }
}
