using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.CommandLineUtils;
using Newtonsoft.Json;
using Rassoodock.Common;

namespace Rassoodock.Cli
{
    public class LinkConfigCommand : ICommandExecution
    {
        public Action<CommandLineApplication> GenerateCommand()
        {
            return command =>
            {
                command.Description = @"
                    Set all configuration values for a particular link. 
                    This will overwrite all of: link-name, database-type, and connection-string";
                var linkName = command.Argument("link-name", "The name of the database link defined earlier");
                var databaseType = command.Argument("database-type",
                    "The type of the database must be one of: (sqlserver, mysql, postgresql, sqlite)");
                var connectionString = command.Argument("connection-string",
                    "The full connection string for this database");
                
                command.OnExecute(() => CreateLinkedDatabaseFolder(linkName.Value, databaseType.Value, connectionString.Value));
            };
        }

        public int CreateLinkedDatabaseFolder(string linkName, string databaseType, string connectionString)
        {
            try
            {
                var directory = Directory.GetCurrentDirectory();
                var folderName = Path.Combine(directory, linkName);


                if (!Directory.Exists(folderName))
                {
                    Console.WriteLine(
                        "The link name you've provided does not exist. Please use the link command to create the link first.");
                    return 1;
                }

                var dbType = databaseType.ParseEnum<DatabaseType>();

                var fileName = Path.Combine(directory, $"{linkName}.json");

                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                using (var file = File.Create(fileName))
                {
                    var database = new LinkedDatabase
                    {
                        DatabaseType = dbType,
                        FolderLocation = folderName,
                        ConnectionString = connectionString,
                        Name = linkName
                    };

                    var json = JsonConvert.SerializeObject(database, Formatting.Indented);
                    var writeBytes = Encoding.UTF8.GetBytes(json);
                    file.Write(writeBytes, 0, writeBytes.Length);
                }
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception caught:");

                Console.WriteLine(JsonConvert.SerializeObject(e, Formatting.Indented));
                return 1;
            }
        }
    }
}
