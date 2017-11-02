using System.Collections.Generic;

namespace Rassoodock.SqlServer.Windows.Models.Domain
{
    public class ForeignKeyConstraint
    {
        public string Name { get; set; }

        public IEnumerable<Column> SourceTableColumns { get; set; }

        public string DestinationTableSchema { get; set; }

        public string DestinationTableName { get; set; }

        public IEnumerable<string> DestinationTableColumnNames { get; set; }

    }
}
