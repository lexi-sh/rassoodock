using System.Collections.Generic;
using System.Data.SqlClient;
using AutoMapper;
using Dapper;
using Rassoodock.Common;
using Rassoodock.Databases;
using Rassoodock.SqlServer.Models.Domain;
using Rassoodock.SqlServer.Models.SqlModels;

namespace Rassoodock.SqlServer
{
    public class TableDatabaseReader
    {
        private readonly LinkedDatabase _database;

        public TableDatabaseReader(LinkedDatabase database)
        {
            _database = database;
        }

        public IEnumerable<Table> GetTables() 
        {
            return Mapper.Map<IEnumerable<Table>>(System.Linq.Enumerable.Empty<SqlServerTable>());
        }

        public IEnumerable<TableSqlModel> GetBaseTableInfo()
        {
            using (var conn = new SqlConnection(_database.ConnectionString))
            {
                conn.Open();
                conn.ChangeDatabase(_database.Name);
                return conn.Query<TableSqlModel>(@"
                    ;WITH filegroups(type_desc, name, data_space_id) as (

                        SELECT DISTINCT au.type_desc, s.name, AU.data_space_id
                        FROM sys.allocation_units au
                        INNER JOIN sys.partitions p ON au.container_id = p.partition_id
                        INNER JOIN sys.data_spaces s ON s.data_space_id = au.data_space_id
                        WHERE au.type_desc in ('IN_ROW_DATA', 'LOB_DATA')
                    )
                    SELECT DISTINCT t.name AS Name, s.name AS SchemaName, tif.name AS TextImageFileGroup, f.name AS FileGroup
                    FROM sys.tables t
                    INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
                    INNER JOIN sys.indexes i ON t.object_id=i.object_id
                    INNER JOIN filegroups tif ON i.data_space_id = tif.data_space_id AND tif.type_desc = 'LOB_DATA'
                    INNER JOIN filegroups f ON i.data_space_id = f.data_space_id AND F.type_desc = 'IN_ROW_DATA'");
            }
        }
    }
}
