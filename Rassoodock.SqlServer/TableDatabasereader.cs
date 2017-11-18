using System.Collections.Generic;
using AutoMapper;
using Rassoodock.Common;
using Rassoodock.Databases;
using Rassoodock.SqlServer.Models.Domain;

namespace Rassoodock.SqlServer
{
    public class TableDatabaseReader
    {
        public IEnumerable<Table> GetTables(LinkedDatabase db) 
        {
            return Mapper.Map<IEnumerable<Table>>(System.Linq.Enumerable.Empty<SqlServerTable>());
        }
    }
}
