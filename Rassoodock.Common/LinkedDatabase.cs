using System;
using System.Collections.Generic;
using System.Text;
using Rassoodock.Common;

namespace Rassoodock
{
    public class LinkedDatabase
    {
        public string Name { get; set; }

        public string ConnectionString { get; set; }

        public DatabaseType DatabaseType { get; set; }

        public string FolderLocation { get; set; }
    }
}
