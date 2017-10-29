namespace Rassoodock.SqlServer.Windows.Models.Domain
{
    public class Column
    {
        public string Name { get; set; }

        public DataType DataType { get; set; }

        public bool Nullable { get; set; }

        public DefaultConstraint Constraint { get; set; }
    }
}
