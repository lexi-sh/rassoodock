﻿using System.Collections.Generic;

namespace Rassoodock.SqlServer.Models.Domain
{
    public class Index
    {
        public Index()
        {
            FileGroup = SqlServerConstants.PrimaryFileGroup;
        }

        public string Name { get; set; }

        public IEnumerable<Column> Columns { get; set; }

        public IEnumerable<Column> IncludedColumns { get; set; }

        public bool Clustered { get; set; }

        public string FilterDefinition { get; set; }

        public string FileGroup { get; set; }
    }
}
