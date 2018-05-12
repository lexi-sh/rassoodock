using System.Text.RegularExpressions;

namespace Rassoodock.SqlServer.Models.Code
{
    public class SqlServerStoredProcedure : ICode
    {
        public string SchemeName { get; set; }

        public string ObjectName { get; set; }

        public string FunctionDefinition { get; set; }

        public string GetApplicationAlteringText()
        {
            return Regex.Replace(FunctionDefinition, "create", "alter", RegexOptions.IgnoreCase);
        }

        public string GetApplicationCreationText()
        {
            return FunctionDefinition;
        }

        public string GetApplicationDeletionText()
        {
            return $"DELETE PROCEDURE [{SchemeName}].[{ObjectName}]";
        }

        public string GetSavingText()
        {
            return FunctionDefinition;
        }
    }
}