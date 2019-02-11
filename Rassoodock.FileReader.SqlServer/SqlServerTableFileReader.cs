using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Rassoodock.SqlServer.Models.Domain;

namespace Rassoodock.FileReader.SqlServer
{
    public class SqlServerTableFileReader
    {
        public SqlServerTable ReadTable(string filePath)
        {
            var text = File.ReadAllText(filePath);
            var fileName = Path.GetFileName(filePath).Split('.');

            return new SqlServerTable();
        }
    }
}
