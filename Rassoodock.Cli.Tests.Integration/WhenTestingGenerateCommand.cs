using System.Data.SqlClient;
using System.IO;
using Dapper;
using Newtonsoft.Json;
using Rassoodock.Common;
using Rassoodock.Databases;
using Rassoodock.Tests.Base;
using Shouldly;
using Xunit;

namespace Rassoodock.Cli.Tests.Integration
{
    public class WhenTestingGenerateCommand : WhenRunningIntegrationTests
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
        public void ShouldCreateStoredProcedures()
        {
            const string databaseType = "sqlserver";
            new LinkCommand().CreateLinkedDatabaseFolder(Database.Name);
            var command = new LinkConfigCommand();
            command.UpdateDatabaseConfig(Database.Name, databaseType, Database.ConnectionString);
            var directory = Directory.GetCurrentDirectory();
            var fileName = Path.Combine(directory, $"{Database.Name}.json");
            var folderName = Path.Combine(directory, Database.Name);
            
            Directory.Exists(folderName).ShouldBe(true);
            File.Exists(fileName).ShouldBe(true);

            var text = File.ReadAllText(fileName);
            var db = JsonConvert.DeserializeObject<LinkedDatabase>(text);
            db.Name.ShouldBe(Database.Name);
            db.FolderLocation.ShouldBe(folderName);
            db.DatabaseType.ShouldBe(DatabaseType.SqlServer);
            db.ConnectionString.ShouldBe(Database.ConnectionString);

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

            var generateCommand = new GenerateCliCommand();
            generateCommand.WriteDatabaseToFolder(Database.Name);

            var storedProc1File = Path.Combine(
                db.FolderLocation,
                "Stored Procedures",
                $"{storedProc1.Schema}.{storedProc1.Name}.sql");

            var storedProc2File = Path.Combine(
                db.FolderLocation,
                "Stored Procedures",
                $"{storedProc2.Schema}.{storedProc2.Name}.sql");

            File.Exists(storedProc1File).ShouldBeTrue();
            File.Exists(storedProc2File).ShouldBeTrue();
        }
    }
}
