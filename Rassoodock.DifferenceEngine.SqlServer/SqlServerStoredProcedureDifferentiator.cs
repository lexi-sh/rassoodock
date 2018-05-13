using System.Collections.Generic;
using System.Text;
using Rassoodock.DifferenceEngine.SqlServer.SqlServerStrategies;
using Rassoodock.SqlServer.Models.Code;
using Rassoodock.SqlServer.Models.Domain;

namespace Rassoodock.DifferenceEngine.SqlServer
{
    public class SqlServerStoredProcedureDifferentiator : IDifferentiator<SqlServerStoredProcedure>
    {
        public string GetDifferenceAlterString(IEnumerable<SqlServerStoredProcedure> objectsFromFileSystem, IEnumerable<SqlServerStoredProcedure> objectsInDb)
        {
            var sb = new StringBuilder();
            sb.AppendLine("--------------------------------");
            sb.AppendLine("--------------------------------");
            sb.AppendLine("----------Rassoodock------------");
            sb.AppendLine("-------Stored Procedures--------");
            sb.AppendLine("--------------------------------");
            sb.AppendLine("--------------------------------");

            var strategies = new SqlServerDifferentiationStrategyBase[]
            {
                new SqlServerStoredProcAdditionDifferentiationStrategy(objectsFromFileSystem, objectsInDb)
            };
            
            foreach (var strategy in strategies)
            {
                sb.AppendLine(strategy.GetDifferenceAlterString());
            }

            return sb.ToString();
        }
    }
}