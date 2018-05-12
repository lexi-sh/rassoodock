using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoreLinq;
using Rassoodock.SqlServer.Extensions;
using Rassoodock.SqlServer.Models.Domain;

namespace Rassoodock.DifferenceEngine.SqlServer.SqlServerStrategies
{
    public class SqlServerColumnAdditionDifferentiationStrategy : SqlServerDifferentiationStrategyBase
    {
        private readonly SqlServerTable _Live;

        private readonly SqlServerTable _SourceControl;

        public SqlServerColumnAdditionDifferentiationStrategy(SqlServerTable sourceControl, SqlServerTable live)
        {
            _SourceControl = sourceControl;
            _Live = live;
        }

        public override string GetDifferenceAlterString()
        {
            var existsInSourceControlAndNotLive = _SourceControl.Columns.ExceptBy(_Live.Columns, x => x.Name);
            var sb = new StringBuilder();
            if (existsInSourceControlAndNotLive.Any()) 
            {
                sb.Append($"ALTER TABLE [{_SourceControl.Schema}].[{_SourceControl.Name}] ADD ");
                foreach (var col in existsInSourceControlAndNotLive)
                {
                    sb.AppendLine();
                    sb.Append("\t");
                    sb.AppendColumn(col);
                    sb.Append(",");
                }
                sb.Length--;
                sb.AppendLine(";");
                sb.AppendLine("GO");
            }
            return sb.ToString();
        }
    }
}
