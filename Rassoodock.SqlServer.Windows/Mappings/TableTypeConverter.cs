using System.Text;
using AutoMapper;
using Rassoodock.Databases;
using Rassoodock.SqlServer.Windows.Extensions;
using Rassoodock.SqlServer.Windows.Models.Domain;

namespace Rassoodock.SqlServer.Windows.Mappings
{
    public class TableTypeConverter : ITypeConverter<SqlServerTable, Table>
    {
        private string OnOrOff(bool b) => b ? "ON" : "OFF";

        public Table Convert(SqlServerTable source, Table destination, ResolutionContext context)
        {
            /*
             * CREATE TABLE [SCHEMA].[TABLE NAME]
                (
                [ColName] [DataType] NOT NULL CONSTRAINT [DfName] DEFAULT (Default Predicate),
                ...
                ) ON [FileGroup] TEXTIMAGE_ON [TextImageFileGroup] (if binary/text/varbinary/largefiletype)
                GO
                SET QUOTED_IDENTIFIER ON/OFF
                GO
                SET ANSI_NULLS ON/OFF
                GO
                CREATE TRIGGER [SCHEMA].[NAME] ON [TABLESCHEMA].[TABLENAME]
                    FOR DELETE/INSERT/UPDATE
                AS
                    STUFF
                GO
                MORE TRIGGERS
                // PRIMARY KEY CONSTRAINT
                ALTER TABLE [TABLESCHEMA].[TABLENAME] ADD CONSTRAINT [PRIMARYKEYNAME] PRIMARY KEY CLUSTERED/NONCLUSTERED  ([COLUMNNAME1], [COLUMNNAME2], ...) ON [PK_FILEGROUP]
                GO
                // INDEX 
                CREATE CLUSTERED/NONCLUSTERED INDEX [NAME] ON [TABLESCHEMA].[TABLENAME] ([COL1],[COL2]) INCLUDE ([COL1],[COL2]) WHERE (WHERE PREDICATE) ON [IX_FILEGROUP]
                GO
                ...
                // FOREIGN KEY
                ALTER TABLE [TABLESCHEMA].[TABLENAME] ADD CONSTRAINT [FK_NAME] FOREIGN KEY ([COL1], [COL2]) REFERENCES [FOREIGN_SCHEMA].[FOREIGN_TABLE] ([FOREIGN_COL1], [FOREIGN_COL2])
                GO
                ...
                // PERMISSIONS
                StateDescription PermissionName ON  [TABLESCHEMA].[TABLENAME] TO [User]
                GO
            */
            var text = new StringBuilder();
            text.AppendLine($"CREATE TABLE [{source.Schema}].[{source.Name}]");
            text.AppendLine("(");
            foreach (var col in source.Columns)
            {
                // Datatype
                text.Append($"[{col.Name}] ");
                if (!string.IsNullOrWhiteSpace(col.DataType.Length))
                {
                    text.Append($"[{col.DataType.ToSql()}({col.DataType.Length})] ");
                }
                else
                {
                    text.Append($"[{col.DataType.ToSql()} ");
                }

                // Nullable
                if (!col.Nullable)
                {
                    text.Append("NOT ");
                }
                text.Append("NULL");

                if (col.DefaultConstraint != null)
                {
                    text.AppendLine($" CONSTRAINT [{col.DefaultConstraint.Name}] DEFAULT ({col.DefaultConstraint.DefaultVlaue})");
                }
                else
                {
                    text.AppendLine();
                }
            }
            text.Append($") ON [{source.FileGroup}]");
            if (source.RequiresTextImageFileGroup())
            {
                text.AppendLine($" TEXTIMAGE_ON {source.TextImageFileGroup}");
            }
            else
            {
                text.AppendLine();
            }
            text.AppendLine("GO");
            //Triggers
            return new Table();
        }
    }
}
