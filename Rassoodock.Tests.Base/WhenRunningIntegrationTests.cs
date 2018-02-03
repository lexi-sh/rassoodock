using System;
using System.Data.SqlClient;
using Dapper;
using Rassoodock.Common;
using Rassoodock.Startup;

namespace Rassoodock.Tests.Base
{
    public class WhenRunningIntegrationTests
    {
        protected readonly LinkedDatabase Database;

        private const string EnvironmentVariableName = "TestSqlServer";

        public WhenRunningIntegrationTests()
        {
            
            // if (Environment.GetEnvironmentVariable(EnvironmentVariableName) == null)
            // {
            //     Environment.SetEnvironmentVariable(
            //         EnvironmentVariableName,
            //         "Data Source=localhost;Integrated Security=True;");
            // }   
            Console.WriteLine("Env var: " + Environment.GetEnvironmentVariable(EnvironmentVariableName));
            Database = new LinkedDatabase
            {
                DatabaseType = DatabaseType.SqlServer,
                ConnectionString = Environment.GetEnvironmentVariable(EnvironmentVariableName),
                Name = EnhancedRandom.String(10, 20)
            };

            using (var conn = new SqlConnection(Database.ConnectionString))
            {
                conn.Open();
                var exists = conn.ExecuteScalar<bool>("SELECT 1 FROM sys.databases WHERE name = @name",
                    new
                    {
                        name = Database.Name
                    });
                if (!exists)
                {
                    conn.Execute($"CREATE DATABASE {Database.Name}");
                }
                else
                {
                    conn.Execute($"DROP DATABASE {Database.Name}");
                    conn.Execute($"CREATE DATABASE {Database.Name}");
                }
            }

            var automapperStartup = new AutomapperStartup();
            automapperStartup.Startup(null);
        }
    }
}
