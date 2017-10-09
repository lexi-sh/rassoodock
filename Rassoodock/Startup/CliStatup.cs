using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.CommandLineUtils;
using Newtonsoft.Json;
using Rassoodock.Common;

namespace Rassoodock.Startup
{
    public class CliStatup : IStartup
    {
        public void Startup(string[] args)
        {
            var app = new CommandLineApplication
            {
                Name = "rassoodock"
            };
            app.HelpOption("-?|-h|--help");

            app.Command("link", command =>
            {
                command.Description = "Set up a database for source control. Note, using this will DESTROY the source control folder of the same name.";
                var databaseName = command.Argument("name",
                    "The named key to reference this database for further usage (e.g. my-sql-server)");

                command.OnExecute(() =>
                {
                    try
                    {

                        var directory = Directory.GetCurrentDirectory();
                        var fileName = Path.Combine(directory, $"{databaseName.Value}.json");
                        var folderName = Path.Combine(directory, databaseName.Value);
                        if (File.Exists(fileName))
                        {
                            File.Delete(fileName);
                        }

                        if (Directory.Exists(folderName))
                        {
                            Directory.Delete(folderName);
                        }

                        Directory.CreateDirectory(folderName);

                        using (var file = File.Create(fileName))
                        {
                            var database = new LinkedDatabase
                            {
                                Name = databaseName.Value,
                                DatabaseType = DatabaseType.NotSetUp,
                                ConnectionString = string.Empty,
                                FolderLocation = folderName
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
                });
            });

            app.Command("link-config", command =>
            {
                command.Description = @"
                    Set all configuration values for a particular link. 
                    This will overwrite all of: link-name, database-type, and connection-string";
                var linkName = command.Argument("link-name", "The name of the database link defined earlier");
                var databaseType = command.Argument("database-type",
                    "The type of the database must be one of: (sqlserver, mysql, postgresql, sqlite)");
                var connectionString = command.Argument("connection-string",
                    "The full connection string for this database");


                command.OnExecute(() =>
                {
                    try
                    {
                        var directory = Directory.GetCurrentDirectory();
                        var folderName = Path.Combine(directory, linkName.Value);


                        if (!Directory.Exists(folderName))
                        {
                            Console.WriteLine(
                                "The link name you've provided does not exist. Please use the link command to create the link first.");
                            return 1;
                        }

                        var dbType = databaseType.Value.ParseEnum<DatabaseType>();

                        var fileName = Path.Combine(directory, $"{linkName.Value}.json");

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
                                ConnectionString = connectionString.Value,
                                Name = linkName.Value
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
                });


            });

            app.Execute(args);
        }
    }
}
