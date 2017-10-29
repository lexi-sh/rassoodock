using System.Collections.Generic;

namespace Rassoodock.SqlServer.Windows.Models.Domain
{
    public class Table
    {
        private const string PrimaryFileGroup = "PRIMARY";
        public Table()
        {
            FileGroup = PrimaryFileGroup;
        }
        public string FileGroup { get; set; }

        public IEnumerable<TableTrigger> Triggers { get; set; }

        public PrimaryKeyConstraint PrimaryKeyConstraint { get; set; }

        public IEnumerable<ForeignKeyConstraint> ForeignKeyConstraints { get; set; }

        public IEnumerable<Index> Indexes { get; set; }

        public IEnumerable<ObjectPermission> PermissionDeclarations { get; set; }
    }
}
