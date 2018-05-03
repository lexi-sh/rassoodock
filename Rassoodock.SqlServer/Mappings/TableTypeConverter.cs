using System.Linq;
using System.Text;
using AutoMapper;
using Rassoodock.Databases;
using Rassoodock.SqlServer.Extensions;
using Rassoodock.SqlServer.Models.Domain;

namespace Rassoodock.SqlServer.Mappings
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
               text.AppendColumn(col);
               text.AppendLine();
            }
            text.Append($") ON [{source.FileGroup}]");
            if (source.RequiresTextImageFileGroup())
            {
                text.AppendLine($" TEXTIMAGE_ON [{source.TextImageFileGroup}]");
            }
            else
            {
                text.AppendLine();
            }
            text.AppendLine("GO");
            
            //Triggers
            foreach (var trigger in source.Triggers)
            {
                text.AppendLine($"SET QUOTED_IDENTIFIER {OnOrOff(trigger.QuotedIdentifier)}");
                text.AppendLine("GO");
                text.AppendLine($"SET ANSI_NULLS {OnOrOff(trigger.QuotedIdentifier)}");
                text.AppendLine("GO");
                text.AppendLine(trigger.Text);
                text.AppendLine("GO");
            }

            //Primary key constraint
            if (source.PrimaryKeyConstraint != null)
            {
                var primaryKeyClusteredString = source.PrimaryKeyConstraint.Clustered ?
                    SqlServerConstants.Clustered :
                    SqlServerConstants.Nonclustered;
                text.Append($"ALTER TABLE [{source.Schema}].[{source.Name}]");
                text.Append($" ADD CONSTRAINT [{source.PrimaryKeyConstraint.Name}] PRIMARY KEY {primaryKeyClusteredString}  ");
                text.AppendColunmNames(source.PrimaryKeyConstraint.Columns);
                text.AppendWithForIndex(source.PrimaryKeyConstraint);
                text.AppendLine($"ON [{source.PrimaryKeyConstraint.FileGroup}]");
                text.AppendLine("GO");
            }

            // Unique Constraints
            foreach (var constraint in source.UniqueConstraints)
            {
                var clusteredString = constraint.Clustered ?
                    SqlServerConstants.Clustered :
                    SqlServerConstants.Nonclustered;

                text.Append($"ALTER TABLE [{source.Schema}].[{source.Name}] ADD CONSTRAINT [{constraint.Name}] ");
                text.Append($"UNIQUE {clusteredString}  ");
                text.AppendColunmNames(constraint.Columns);
                text.AppendLine($"ON [{constraint.FileGroup}]");
                text.AppendLine("GO");
            }


            // Indexes
            foreach (var index in source.Indexes)
            {
                var clusteredString = index.Clustered ?
                    SqlServerConstants.Clustered :
                    SqlServerConstants.Nonclustered;

                text.Append($"CREATE {clusteredString} INDEX [{index.Name}] ");
                text.Append($"ON [{source.Schema}].[{source.Name}] ");

                text.AppendColunmNames(index.Columns);

                if (index.IncludedColumns.Any())
                {
                    text.Append("INCLUDE ");
                    text.AppendColunmNames(index.IncludedColumns);
                }

                if (string.IsNullOrWhiteSpace(index.FilterDefinition))
                {
                    text.Append($"WHERE ({index.FilterDefinition})");
                }
                text.AppendWithForIndex(index);
                text.AppendLine($"ON [{index.FileGroup}]");
                text.AppendLine("GO");
            }

            // Foreign Keys
            foreach (var constraint in source.ForeignKeyConstraints)
            {
                text.Append($"ALTER TABLE [{source.Schema}].[{source.Name}] ADD CONSTRAINT [{constraint.Name}] ");
                text.Append("FOREIGN KEY ");
                text.AppendColunmNames(constraint.SourceTableColumns);
                text.Append($"REFERENCES [{constraint.DestinationTableSchema}].[{constraint.DestinationTableName}] ");
                text.AppendColunmNames(constraint.DestinationTableColumnNames);
                text.AppendLine();
                text.AppendLine("GO");
            }

            // Permission grants
            foreach (var permission in source.PermissionDeclarations)
            {
                text.Append($"{permission.StateDescription} {permission.PermissionName} ON  ");
                text.AppendLine($"[{source.Schema}].[{source.Name}] TO [{permission.User}] ");
                text.AppendLine("GO");
            }

            return new Table
            {
                Name = source.Name,
                Schema = source.Schema,
                Text = text.ToString()
            };
        }
    }
}
