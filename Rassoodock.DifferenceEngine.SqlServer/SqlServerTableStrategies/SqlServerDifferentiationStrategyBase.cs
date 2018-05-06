namespace Rassoodock.DifferenceEngine.SqlServer.SqlServerTableStrategies
{
    public abstract class SqlServerDifferentiationStrategyBase
    {
        public abstract string GetDifferenceAlterString();
    }
}