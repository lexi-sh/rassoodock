using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.CommandLineUtils;
using Newtonsoft.Json;
using Rassoodock.Common;
using Rassoodock.Databases;
using Rassoodock.Databases.Services;

namespace Rassoodock.Cli
{
    public class MigrateCommand : ICommandExecution
    {
        public Action<CommandLineApplication> GenerateCommand()
        {
            return command =>
            {
                command.Description =
                    "Generate a migration script to apply to your database from the source control version. Writes to system out.";
                command.HelpOption("-?|-h|--help");

                var databaseName = command.Argument("name",
                    "The named key to reference this database for further usage (e.g. some-sql-server)");
                
                command.OnExecute(() => GenerateMigrationScript(databaseName.Value));
            };
        }

        private int GenerateMigrationScript(string databaseName)
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

                var savableObjects = new Dictionary<string, IEnumerable<SavableDatabaseObject>>()
                {
                    [SubfolderService.GetStoredProceduresFolder(database)] = db.StoredProcedures,
                    [SubfolderService.GetTableFolder(database)] = db.Tables,
                };

                foreach (var savableObjectKvp in savableObjects)
                {
                    foreach (var savableObject in savableObjectKvp.Value)
                    {
                        var sqlFileName = Path.Combine(savableObjectKvp.Key, $"{savableObject.Schema}.{savableObject.Name}.sql");

                        if (File.Exists(sqlFileName))
                        {
                            File.Delete(sqlFileName);
                        }
                        File.WriteAllText(sqlFileName, savableObject.Text);
                    }

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