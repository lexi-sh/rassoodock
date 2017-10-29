namespace Rassoodock.SqlServer.Windows.Models.Domain
{
    public class TableTrigger
    {
        public TriggerAction Action { get; set; }

        public string Schema { get; set; }

        public string Name { get; set; }

        public string TableSchema { get; set; }

        public string TableName { get; set; }

        public bool QuotedIdentifier { get; set; }

        public bool AnsiNulls { get; set; }

        public bool NoCount { get; set; }
    }
}
