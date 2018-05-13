using Rassoodock.SqlServer.Models.Code;
using System.IO;

namespace Rassoodock.FileReader.SqlServer
{
    public class SqlServerStoredProceduresFileReader
    {
        public SqlServerStoredProcedure ReadStoredProcedure(string filePath)
        {
            var text = File.ReadAllText(filePath);
            var fileName = Path.GetFileName(filePath).Split('.');

            return new SqlServerStoredProcedure
            {
                SchemaName = fileName[0],
                ObjectName = fileName[1],
                FunctionDefinition = text
            };
        }
    }
}
