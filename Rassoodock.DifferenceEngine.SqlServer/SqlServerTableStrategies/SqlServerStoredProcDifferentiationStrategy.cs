using Rassoodock.SqlServer.Models.Domain;

namespace Rassoodock.DifferenceEngine.SqlServer.SqlServerTableStrategies
{
    public class SqlServerStoredProcDifferentiationStrategy : SqlServerDifferentiationStrategyBase
    {
        private readonly SqlServerTable _Live;

        private readonly SqlServerTable _SourceControl;

        public SqlServerStoredProcDifferentiationStrategy(SqlServerTable sourceControl, SqlServerTable live)
        {
            _SourceControl = sourceControl;
            _Live = live;
        }

        public override string GetDifferenceAlterString()
        {
            throw new System.NotImplementedException();
        }
    }
}