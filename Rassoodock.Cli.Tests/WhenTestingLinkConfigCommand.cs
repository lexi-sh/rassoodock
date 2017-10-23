using System.IO;
using Newtonsoft.Json;
using Rassoodock.Common;
using Shouldly;
using Xunit;

namespace Rassoodock.Cli.Tests
{
    public class WhenTestingLinkConfigCommand
    {
        [Fact]
        public void ShouldUpdateJsonConfig()
        {
            const string databaseName = "testing123";
            const string databaseType = "sqlserver";
            const string connectionString = "Data Source=localhost;Integrated Security=True;";
            new LinkCommand().CreateLinkedDatabaseFolder(databaseName);
            var command = new LinkConfigCommand();
            command.UpdateDatabaseConfig(databaseName, databaseType, connectionString);
            var directory = Directory.GetCurrentDirectory();
            var fileName = Path.Combine(directory, $"{databaseName}.json");
            var folderName = Path.Combine(directory, databaseName);
            
            Directory.Exists(folderName).ShouldBe(true);
            File.Exists(fileName).ShouldBe(true);

            var text = File.ReadAllText(fileName);
            var db = JsonConvert.DeserializeObject<LinkedDatabase>(text);
            db.Name.ShouldBe(databaseName);
            db.FolderLocation.ShouldBe(folderName);
            db.DatabaseType.ShouldBe(DatabaseType.SqlServer);
            db.ConnectionString.ShouldBe(connectionString);
        }
    }
}
