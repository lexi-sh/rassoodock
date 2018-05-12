namespace Rassoodock.SqlServer.Models.Code
{
    public interface ICode
    {
        string SchemeName { get; }

        string ObjectName { get; }

        string FunctionDefinition { get; }

        string GetSavingText();

        string GetApplicationAlteringText();

        string GetApplicationCreationText();

        string GetApplicationDeletionText();
    }
}