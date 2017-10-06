
using System;
using Rassoodock.Common;
using Rassoodock.Tests.Utilities;
using Xunit;

namespace Rassoodock.SqlServer.Windows.Tests
{
    public class WhenGettingStoredProcedures
    {
        private readonly LinkedDatabase _Database;

        public WhenGettingStoredProcedures()
        {
            LaunchSettings.SetLaunchSettingsEnvironmentVariables();
            _Database = new LinkedDatabase
            {
                DatabaseType = DatabaseType.SqlServer,
                ConnectionString = Environment.GetEnvironmentVariable("TestWindowsSqlServer")
            };
        }


        [Fact]
        public void ShouldBeAbleToGetStoredProceduresForWindowsSqlServer()
        {
            var dbReader = new DatabaseReader();
            var storedProcs = dbReader.GetStoredProcedures(_Database);
        }
    }
}
