
using System;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Rassoodock.Common;
using Rassoodock.Databases;
using Rassoodock.Tests.Utilities;
using Shouldly;
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

            var query = 
                $"IF NOT EXISTS(SELECT 1 FROM sys.databases WHERE name='{_database.Name}') BEGIN CREATE DATABASE {_database.Name}; END";
            
            using (var conn = new SqlConnection(_database.ConnectionString))
            {
                conn.Open();
                var exists = conn.ExecuteScalar<bool>("SELECT 1 FROM sys.databases WHERE name = @name",
                    new
                    {
                        name = _database.Name
                    });
                if (!exists)
                {
                    conn.Execute($"CREATE DATABASE {_database.Name}");
                }
            }
        }

        private void CreateStoredProc(StoredProcedure procedure)
        {
            var name = $"[{procedure.Schema}].[{procedure.Name}]";
            var query = $"CREATE PROC {name} AS BEGIN {procedure.Text} END";
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

            var storedProc1 = new StoredProcedure
            {
                Schema = "dbo",
                Name = "asd123",
                Text = "SELECT 1"
            };

            var storedProc2 = new StoredProcedure
            {
                Schema = "dbo",
                Name = "asd1444",
                Text = "SELECT 'asd'"
            };

            CreateStoredProc(storedProc1);
            CreateStoredProc(storedProc2);

            var storedProcs = dbReader.GetStoredProcedures().ToList();
            storedProcs.FirstOrDefault(x => x.Text == storedProc1.Text).ShouldNotBeNull();
            storedProcs.FirstOrDefault(x => x.Text == storedProc2.Text).ShouldNotBeNull();
        }
    }
}
