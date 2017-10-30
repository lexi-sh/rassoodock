using System;
using System.Collections.Generic;
using System.Text;
using Rassoodock.SqlServer.Windows.Models.Domain;

namespace Rassoodock.SqlServer.Windows.Extensions
{
    public static class TableExtensions
    {
        public static bool RequiresTextImageFileGroup(this SqlServerTable table)
        {
            var requiresTextImageFileGroup = false;
            foreach (var col in table.Columns)
            {
                if (col.DataType.DataType == DataType.Text ||
                    col.DataType.DataType == DataType.NText ||
                    col.DataType.DataType == DataType.Image ||
                    col.DataType.DataType == DataType.Xml)
                {
                    requiresTextImageFileGroup = true;
                }

                if (col.DataType.DataType == DataType.VarChar ||
                    col.DataType.DataType == DataType.NVarChar ||
                    col.DataType.DataType == DataType.VarBinary)
                {
                    if (col.DataType.Length == SqlServerConstants.MaxLength)
                    {
                        requiresTextImageFileGroup = true;
                    }
                }
            }
            return requiresTextImageFileGroup;
        }
    }
}
