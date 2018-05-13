using System.Collections.Generic;
using AutoMapper;
using Rassoodock.Common;
using Rassoodock.Databases;

namespace Rassoodock.SqlServer
{
    public class SqlServerDatabaseReader : IDatabaseReader
    {
        private readonly LinkedDatabase _database;

        private readonly TableDatabaseReader _tableReader;

        private readonly StoredProcedureDatabaseReader _spReader;

        public SqlServerDatabaseReader(LinkedDatabase database)
        {
            _database = database;
            _tableReader = new TableDatabaseReader(database);
            _spReader = new StoredProcedureDatabaseReader(database);
        }

        public IEnumerable<StoredProcedure> GetStoredProcedures() => Mapper.Map<IEnumerable<StoredProcedure>>(_spReader.GetStoredProcedures());

        public IEnumerable<Table> GetTables() => Mapper.Map<IEnumerable<Table>>(_tableReader.GetTables());
    }
}
