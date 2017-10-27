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
                command.Description = @"Set all configuration values for a particular link. This will overwrite all of: link-name, database-type, and connection-string";
                command.HelpOption("-?|-h|--help");
                var linkName = command.Option("-n|--name", "The name of the database link defined earlier", CommandOptionType.SingleValue);
                var databaseType = command.Option("-d|--database-type",
                    "The type of the database must be one of: (sqlserver, mysql, postgresql, sqlite)", CommandOptionType.SingleValue);
                var connectionString = command.Option("-c|--connection-string",
                    "The full connection string for this database", CommandOptionType.SingleValue);
                
                command.OnExecute(() => UpdateDatabaseConfig(linkName.Value(), databaseType.Value(), connectionString.Value()));
            };
        }

        public int UpdateDatabaseConfig(string linkName, string databaseType, string connectionString)
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

                if (string.IsNullOrEmpty(databaseType))
                {
                    Console.WriteLine("Database type required for modifying configuration");
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
                Console.WriteLine($"Exception caught:{e.Message}");
                
                return 1;
            }
        }
    }
}
