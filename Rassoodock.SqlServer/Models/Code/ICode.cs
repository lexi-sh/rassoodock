namespace Rassoodock.SqlServer.Models.Code
{
    public interface ICode
    {
        string SchemaName { get; }

        string ObjectName { get; }

        string FunctionDefinition { get; }

        string GetSourceControlSavableText();

        string GetApplicationAlteringText();

        string GetApplicationCreationText();

        string GetApplicationDeletionText();
    }
}