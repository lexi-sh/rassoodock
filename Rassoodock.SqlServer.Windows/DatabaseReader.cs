using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using AutoMapper;
using Dapper;
using Rassoodock.Common;
using Rassoodock.Databases;
using Rassoodock.SqlServer.Windows.Models;

namespace Rassoodock.SqlServer.Windows
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
                    SELECT ROUTINE_DEFINITION, 
                           SPECIFIC_SCHEMA, 
                           SPECIFIC_NAME 
                    FROM INFORMATION_SCHEMA.ROUTINES");

                return Mapper.Map<IEnumerable<StoredProcedure>>(sqlModels);
            }
        }
    }
}
