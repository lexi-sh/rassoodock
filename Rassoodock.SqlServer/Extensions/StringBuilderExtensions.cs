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

        public static void AppendWithForIndex(this StringBuilder text, Index index) 
        {
            var temporarySb = new StringBuilder();
            var withNecessary = false;
            temporarySb.Append("WITH (");
            
            if (index.PadIndex) 
            {
                withNecessary = true;
                temporarySb.Append("PAD_INDEX = ON, ");
            }
            if (index.StatisticsNoRecompute) 
            {
                withNecessary = true;
                temporarySb.Append("STATISTICS_NORECOMPUTE = ON, ");
            }
            if (index.IgnoreDuplicateKey) 
            {
                withNecessary = true;
                temporarySb.Append("IGNORE_DUP_KEY = ON, ");
            }
            if (!index.AllowRowLocks) 
            {
                withNecessary = true;
                temporarySb.Append("ALLOW_ROW_LOCKS = OFF, ");
            }
            if (!index.AllowPageLocks) 
            {
                withNecessary = true;
                temporarySb.Append("ALLOW_PAGE_LOCKS = OFF, ");
            }
            if (index.FillFactor != default(int)) 
            {
                withNecessary = true;
                temporarySb.Append($"FILLFACTOR = {index.FillFactor}, ");
            }
            temporarySb.Length -= 2;
            temporarySb.Append(") ");

            if (withNecessary) 
            {
                text.Append(temporarySb);
            }
        }
    }
}
