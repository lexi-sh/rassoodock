using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rassoodock.Databases;

namespace Rassoodock.DifferenceEngine
{
    public class StoredProcedureDifferentiator : IDifferentiator<StoredProcedure>
    {
        public string GetDifferenceAlterString(IEnumerable<StoredProcedure> objectsFromFileSystem, IEnumerable<StoredProcedure> objectsInDb)
        {
            var objects = from sp1 in objectsFromFileSystem
                join sp2 in objectsInDb
                on $"{sp1.Schema}.{sp1.Name}" equals $"{sp2.Schema}.{sp2.Name}"
                select new
                {
                    FileSystemObject = sp1,
                    DbObject = sp2
                };

            var retString = new StringBuilder();
            retString.AppendLine("------------------");
            retString.AppendLine("Rassoodock: Stored Procedures");
            retString.AppendLine("------------------");


            foreach (var obj in objects)
            {
                if (DidChange(obj.DbObject, obj.FileSystemObject))
                {
                    retString.AppendLine($"ALTER PROCEDURE [{obj.DbObject.Schema}].[{obj.DbObject.Name}]");
                }
            }

            return string.Empty;
        }

        private bool DidChange(StoredProcedure sp1, StoredProcedure sp2)
        {
            return sp1.Text != sp2.Text;
        }
    }
}
