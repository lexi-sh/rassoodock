using System.Collections.Generic;

namespace Rassoodock.SqlServer.Models.Domain
{
    public class Index
    {
        public Index()
        {
            FileGroup = SqlServerConstants.PrimaryFileGroup;
            AllowRowLocks = true;
            AllowPageLocks = true;
        }

        public string Name { get; set; }

        public IEnumerable<Column> Columns { get; set; }

        public IEnumerable<Column> IncludedColumns { get; set; }

        public bool Clustered { get; set; }

        public string FilterDefinition { get; set; }

        public string FileGroup { get; set; }

        public bool PadIndex { get; set; }

        public bool StatisticsNoRecompute { get; set; }

        public bool IgnoreDuplicateKey { get; set; }

        public bool AllowRowLocks { get; set; }

        public bool AllowPageLocks { get; set; }

        public int FillFactor { get; set; }
    }
}
