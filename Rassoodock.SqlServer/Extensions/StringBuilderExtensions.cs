using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rassoodock.SqlServer.Models.Domain;

namespace Rassoodock.SqlServer.Extensions
{
    public static class StringBuilderExtensions
    {
        public static void AppendColunmNames(this StringBuilder text, IEnumerable<Column> columns)
        {
            AppendColunmNames(text, columns.Select(x => x.Name));
        }
        public static void AppendColunmNames(this StringBuilder text, IEnumerable<string> columns)
        {
            text.Append("(");
            foreach (var column in columns)
            {
                text.Append($"[{column}], ");
            }
            text.Length -= 2;
            text.Append(") ");
        }
    }
}
