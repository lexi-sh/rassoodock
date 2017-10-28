﻿using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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
                    SELECT SPECIFIC_SCHEMA, 
                           SPECIFIC_NAME 
                    FROM INFORMATION_SCHEMA.ROUTINES
                    WHERE ROUTINE_TYPE = 'PROCEDURE'");

                foreach (var routine in sqlModels)
                {
                    var storedProcText = conn.Query<SpHelpTextSqlModel>("sp_helptext @name",
                        new
                        {
                            name = $"{routine.Specific_Schema}.{routine.Specific_Name}"
                        });

                    routine.Routine_Definition = string.Join("", storedProcText.Select(x => x.Text));
                }

                return Mapper.Map<IEnumerable<StoredProcedure>>(sqlModels);
            }
        }
    }
}
