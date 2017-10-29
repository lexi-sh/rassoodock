using System.Collections.Generic;

namespace Rassoodock.SqlServer.Windows.Models.Domain
{
    public class Index
    {
        public string Name { get; set; }

        public IEnumerable<Column> Columns { get; set; }

        public IEnumerable<Column> IncludedColumns { get; set; }

        public bool Clustered { get; set; }

        public string FilterDefinition { get; set; }
    }
}
