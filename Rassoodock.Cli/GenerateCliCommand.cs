using System;
using System.IO;
using Microsoft.Extensions.CommandLineUtils;
using Newtonsoft.Json;
using Rassoodock.Common;
using Rassoodock.Databases.Services;

namespace Rassoodock.Cli
{
    public class GenerateCliCommand : ICommandExecution
    {
        public Action<CommandLineApplication> GenerateCommand()
        {
            return command =>
            {
                command.Description =
                    "Generate the scripts in the folder to be committed at a later time.";
                command.HelpOption("-?|-h|--help");
                var databaseName = command.Argument("name",
                    "The named key to reference this database for further usage (e.g. some-sql-server)");

                command.OnExecute(() => WriteDatabaseToFolder(databaseName.Value));
            };
        }

        public int WriteDatabaseToFolder(string databaseName)
        {
            try
            {

                var directory = Directory.GetCurrentDirectory();
                var fileName = Path.Combine(directory, $"{databaseName}.json");
                var folderName = Path.Combine(directory, databaseName);
                
                if (!Directory.Exists(folderName))
                {
                    Console.WriteLine("Database not found!");
                    return 1;
                }

                var database = JsonConvert.DeserializeObject<LinkedDatabase>(File.ReadAllText(fileName));

                var dbFactory = DatabaseGenerationStrategyFactory.GetDatabaseGenerationStrategyBase(database);
                var db = dbFactory.GetDatabase(database);
                // This will need to be a dependency graph shortly and should be in a separate service
                // But for now, let's do a POC here
                var storedProcs = SubfolderService.GetStoredProceduresFolder(database);
                foreach (var storedProc in db.StoredProcedures)
                {
                    var spFileName = Path.Combine(storedProcs, $"{storedProc.Schema}.{storedProc.Name}.sql");

                    if (File.Exists(spFileName))
                    {
                        File.Delete(spFileName);
                    }
                    File.WriteAllText(spFileName, storedProc.Text);
                }

                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception caught: {e.Message}");
                return 1;
            }
        }
    }
}
