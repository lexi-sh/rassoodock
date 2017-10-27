using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Rassoodock.Databases;
using Rassoodock.Tests.Base;
using Shouldly;
using Xunit;

namespace Rassoodock.SqlServer.Windows.Tests.Integration
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
            storedProcs.FirstOrDefault(x => x.Text == StoredProcString(storedProc1)).ShouldNotBeNull();
            storedProcs.FirstOrDefault(x => x.Text == StoredProcString(storedProc2)).ShouldNotBeNull();
        }
    }
}
