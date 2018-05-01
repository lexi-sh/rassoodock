using System.Collections.Generic;
using Rassoodock.SqlServer.Models.Domain;

namespace Rassoodock.DifferenceEngine.SqlServer.SqlServerTableStrategies
{
    public class SqlServerColumnDifferentiationStrategy
    {
        private readonly IEnumerable<Column> _Live;

        private readonly IEnumerable<Column> _SourceControl;

        public SqlServerColumnDifferentiationStrategy(IEnumerable<Column> sourceControl, IEnumerable<Column> live)
        {
            _SourceControl = sourceControl;
            _Live = live;
        }

        public string GetDifferenceAlterString()
        {
            return "";
        }
    }
}
