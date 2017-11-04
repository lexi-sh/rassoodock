using System.Collections.Generic;

namespace Rassoodock.SqlServer.Models.Domain
{
    public class UniqueConstraint
    {
        public UniqueConstraint()
        {
            FileGroup = SqlServerConstants.PrimaryFileGroup;
        }

        public string Name { get; set; }

        public IEnumerable<Column> Columns { get; set; }
        
        public string FileGroup { get; set; }

        public bool Clustered { get; set; }

    }
}
