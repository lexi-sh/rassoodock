using System.Collections.Generic;
using System.Data.SqlClient;
using AutoMapper;
using Dapper;
using Rassoodock.Common;
using Rassoodock.Databases;
using Rassoodock.SqlServer.Models.Code;
using Rassoodock.SqlServer.Models.SqlModels;

namespace Rassoodock.SqlServer
{
    public class DatabaseReader : IDatabaseReader
    {
        private readonly LinkedDatabase _database;

        public DatabaseReader(LinkedDatabase database)
        {
            _database = database;
        }

        public IEnumerable<StoredProcedure> GetStoredProcedures()
        {
            using (var conn = new SqlConnection(_database.ConnectionString))
            {
                conn.Open();
                conn.ChangeDatabase(_database.Name);
                var sqlModels = conn.Query<RoutinesSqlModel>(@"
                    SELECT r.SPECIFIC_SCHEMA, 
                           r.SPECIFIC_NAME ,
                           s.uses_ansi_nulls, 
                           s.uses_quoted_identifier,
                           s.definition
                    FROM INFORMATION_SCHEMA.ROUTINES r
                        INNER JOIN SYS.SQL_MODULES s
                            ON OBJECT_NAME(s.object_id) = r.SPECIFIC_NAME
                    WHERE ROUTINE_TYPE = 'PROCEDURE'");
                
                return Mapper.Map<IEnumerable<StoredProcedure>>(
                    Mapper.Map<IEnumerable<SqlServerStoredProcedure>>(sqlModels));
            }
        }
    }
}
