namespace Rassoodock.SqlServer.Windows.Models.Domain
{
    public class Column
    {
        public string Name { get; set; }

        public DataTypeCls DataType { get; set; }

        public bool Nullable { get; set; }

        public DefaultConstraint DefaultConstraint { get; set; }

        public string Collation { get; set; }
    }
}
