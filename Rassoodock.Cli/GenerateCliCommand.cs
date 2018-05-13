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
                if(string.IsNullOrWhiteSpace(databaseName))
                {
                    Console.WriteLine("Nickname of database required (See help for details)");
                    return 1;
                }
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
                Console.WriteLine(e.StackTrace);
                return 1;
            }
        }
    }
}
