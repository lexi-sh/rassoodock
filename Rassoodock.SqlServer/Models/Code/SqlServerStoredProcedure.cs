using System;
using System.Text.RegularExpressions;

namespace Rassoodock.SqlServer.Models.Code
{
    public class SqlServerStoredProcedure : ICode, IEquatable<SqlServerStoredProcedure>
    {
        public string SchemaName { get; set; }

        public string ObjectName { get; set; }

        public string FunctionDefinition { get; set; }

        public bool Equals(SqlServerStoredProcedure other)
        {
            return SchemaName == other?.SchemaName
                && ObjectName == other?.ObjectName
                && FunctionDefinition == other?.FunctionDefinition;
        }

        public override int GetHashCode()
        {
            return string.Concat(SchemaName, ObjectName, FunctionDefinition).GetHashCode();
        }

        public string GetApplicationAlteringText()
        {
            return Regex.Replace(FunctionDefinition, "create", "ALTER", RegexOptions.IgnoreCase);
        }

        public string GetApplicationCreationText()
        {
            return FunctionDefinition;
        }

        public string GetApplicationDeletionText()
        {
            return $"DELETE PROCEDURE [{SchemaName}].[{ObjectName}]";
        }

        public string GetSourceControlSavableText()
        {
            return FunctionDefinition;
        }
    }
}