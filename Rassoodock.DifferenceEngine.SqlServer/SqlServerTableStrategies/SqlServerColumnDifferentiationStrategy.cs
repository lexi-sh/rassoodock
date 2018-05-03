using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoreLinq;
using Rassoodock.SqlServer.Extensions;
using Rassoodock.SqlServer.Models.Domain;

namespace Rassoodock.DifferenceEngine.SqlServer.SqlServerTableStrategies
{
    public class SqlServerColumnDifferentiationStrategy
    {
        private readonly SqlServerTable _Live;

        private readonly SqlServerTable _SourceControl;

        public SqlServerColumnDifferentiationStrategy(SqlServerTable sourceControl, SqlServerTable live)
        {
            _SourceControl = sourceControl;
            _Live = live;
        }

        public string GetDifferenceAlterString()
        {
            var existsInSourceControlAndNotLive = _SourceControl.Columns.ExceptBy(_Live.Columns, x => x.Name);
            var sb = new StringBuilder();
            if (existsInSourceControlAndNotLive.Any()) 
            {
                sb.AppendLine($"ALTER TABLE [{_SourceControl.Schema}].[{_SourceControl.Name}] ADD ");
                foreach (var col in existsInSourceControlAndNotLive)
                {
                    sb.AppendColumn(col);
                    sb.AppendLine(",");
                }
                sb.Length--;
                sb.AppendLine("GO");
            }
            return "";
        }
    }
}
