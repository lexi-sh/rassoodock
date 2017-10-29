using System.Collections.Generic;

namespace Rassoodock.SqlServer.Windows.Models.Domain
{
    public class PrimaryKeyConstraint
    {
        public string Name { get; set; }

        public bool Clustered { get; set; }

        public IEnumerable<Column> Columns { get; set; }
    }
}
