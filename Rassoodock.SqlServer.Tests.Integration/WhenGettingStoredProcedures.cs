using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Rassoodock.Databases;
using Rassoodock.Tests.Base;
using Shouldly;
using Xunit;

namespace Rassoodock.SqlServer.Tests.Integration
{
    public class WhenGettingStoredProcedures : WhenRunningIntegrationTests
    {
        private void CreateStoredProc(StoredProcedure procedure)
        {
            var query = StoredProcString(procedure);
            using (var conn = new SqlConnection(Database.ConnectionString))
            {
                conn.Open();
                conn.ChangeDatabase(Database.Name);
                conn.Execute(query);
            }
        }

        private string StoredProcString(StoredProcedure procedure)
        {
            var name = $"[{procedure.Schema}].[{procedure.Name}]";
            return $"CREATE PROC {name} AS BEGIN {procedure.Text} END";
        }
        
        [Fact]
        public void ShouldBeAbleToGetStoredProceduresForWindowsSqlServer()
        {
            var dbReader = new DatabaseReader(Database);

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

            var sp1 = storedProcs.FirstOrDefault(x => x.Name == storedProc1.Name);
            sp1.ShouldNotBeNull();
            sp1.Text.ShouldContain(storedProc1.Text);

            var sp2 = storedProcs.FirstOrDefault(x => x.Name == storedProc2.Name);
            sp2.ShouldNotBeNull();
            sp2.Text.ShouldContain(storedProc2.Text);
        }

        [Fact]
        public void ShouldNotCutOffStoredProcAfter8000Characters()
        {
            var dbReader = new DatabaseReader(Database);

            var storedProc1 = new StoredProcedure
            {
                Schema = "dbo",
                Name = "long stored proc",
                Text = $"SELECT '{EnhancedRandom.String(8100, 10000)}'"
            };

            CreateStoredProc(storedProc1);
            
            var storedProcs = dbReader.GetStoredProcedures().ToList();
            var sp1 = storedProcs.FirstOrDefault(x => x.Name == storedProc1.Name);
            sp1.ShouldNotBeNull();
            sp1.Text.ShouldContain(storedProc1.Text);
        }

        [Fact]
        public void ShouldSetAnsiNullsAndQuotedIdentifierCorrectly()
        {
            var dbReader = new DatabaseReader(Database);

            var procedure = new StoredProcedure
            {
                Schema = "dbo",
                Name = "ansi and quoted",
                Text = $"SELECT '{EnhancedRandom.String(800, 1000)}'"
            };

            var query = StoredProcString(procedure);
            using (var conn = new SqlConnection(Database.ConnectionString))
            {
                conn.Open();
                conn.ChangeDatabase(Database.Name);
                conn.Execute("SET QUOTED_IDENTIFIER OFF");
                conn.Execute("SET ANSI_NULLS OFF");
                conn.Execute(query);
            }

            var storedProcs = dbReader.GetStoredProcedures().ToList();
            var sp1 = storedProcs.FirstOrDefault(x => x.Name == procedure.Name);
            sp1.ShouldNotBeNull();
            sp1.Text.ShouldContain(procedure.Text);
            sp1.Text.ShouldContain("SET QUOTED_IDENTIFIER OFF");
            sp1.Text.ShouldContain("SET ANSI_NULLS OFF");
        }
    }
}
