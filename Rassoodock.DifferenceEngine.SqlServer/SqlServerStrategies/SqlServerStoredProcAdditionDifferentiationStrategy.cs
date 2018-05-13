using System.Collections.Generic;
using System.Text;
using MoreLinq;
using Rassoodock.SqlServer.Models.Code;
using Rassoodock.SqlServer.Models.Domain;

namespace Rassoodock.DifferenceEngine.SqlServer.SqlServerStrategies
{
    public class SqlServerStoredProcAdditionDifferentiationStrategy : SqlServerDifferentiationStrategyBase
    {
        private readonly IEnumerable<SqlServerStoredProcedure> _Live;

        private readonly IEnumerable<SqlServerStoredProcedure> _SourceControl;

        public SqlServerStoredProcAdditionDifferentiationStrategy(
            IEnumerable<SqlServerStoredProcedure> sourceControl,
            IEnumerable<SqlServerStoredProcedure> live)
        {
            _SourceControl = sourceControl;
            _Live = live;
        }

        public override string GetDifferenceAlterString()
        {
            var sb = new StringBuilder();
            var existsInSourceControlAndNotLive = _SourceControl.ExceptBy(_Live, x => $"[{x.ObjectName}].[{x.SchemaName}]");

            foreach(var proc in existsInSourceControlAndNotLive)
            {
                sb.AppendLine(proc.GetApplicationCreationText());
                sb.AppendLine("GO");
            }
            return sb.ToString();
        }
    }
}