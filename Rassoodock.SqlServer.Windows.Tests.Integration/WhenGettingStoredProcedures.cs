
using System;
using System.Data.SqlClient;
using Dapper;
using Rassoodock.Common;
using Rassoodock.Tests.Utilities;
using Xunit;

namespace Rassoodock.SqlServer.Windows.Tests.Integration
{
    public class WhenGettingStoredProcedures
    {
        private readonly LinkedDatabase _database;

        public WhenGettingStoredProcedures()
        {
            LaunchSettings.SetLaunchSettingsEnvironmentVariables();
            _database = new LinkedDatabase
            {
                DatabaseType = DatabaseType.SqlServer,
                ConnectionString = Environment.GetEnvironmentVariable("TestWindowsSqlServer"),
                Name = "TestDatabase"
            };

            var query = $@"IF EXISTS(SELECT * FROM sys.databases WHERE name='{_database.Name}')
                DROP DATABASE {_database.Name};
                CREATE DATABASE {_database.Name};";

            using (var conn = new SqlConnection(_database.ConnectionString))
            {
                conn.Open();
                conn.Execute(query);
            }
        }
        
        [Fact]
        public void ShouldBeAbleToGetStoredProceduresForWindowsSqlServer()
        {
            var dbReader = new DatabaseReader(_database);
            var storedProcs = dbReader.GetStoredProcedures();

        }
    }
}
