namespace Rassoodock.SqlServer.Models.Domain
{
    public class DefaultConstraint
    {
        // Examples: (0), ('default'), sysutcdatetime(), NEXT VALUE FOR DBO.SEQUENCE
        public string DefaultVlaue { get; set; }

        public string Name { get; set; }
    }
}
