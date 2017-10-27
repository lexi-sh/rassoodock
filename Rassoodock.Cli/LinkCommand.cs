using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.CommandLineUtils;
using Newtonsoft.Json;
using Rassoodock.Common;

namespace Rassoodock.Cli
{
    public class LinkCommand : ICommandExecution
    {
        public Action<CommandLineApplication> GenerateCommand()
        {
            return command =>
            {
                command.HelpOption("-?|-h|--help");
                command.Description =
                    "Set up a database for source control. Note, using this will DESTROY the source control folder of the same name.";
                var databaseName = command.Argument("name",
                    "The named key to reference this database for further usage (e.g. some-sql-server)");

                command.OnExecute(() => CreateLinkedDatabaseFolder(databaseName.Value));
            };
        }

        public int CreateLinkedDatabaseFolder(string databaseName)
        {
            try
            {

                var directory = Directory.GetCurrentDirectory();
                var fileName = Path.Combine(directory, $"{databaseName}.json");
                var folderName = Path.Combine(directory, databaseName);
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                if (Directory.Exists(folderName))
                {
                    Directory.Delete(folderName, true);
                }

                Directory.CreateDirectory(folderName);

                using (var file = File.Create(fileName))
                {
                    var database = new LinkedDatabase
                    {
                        Name = databaseName,
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
                Console.WriteLine("Exception caught:" + e.Message);
                return 1;
            }
        }
    }
}
