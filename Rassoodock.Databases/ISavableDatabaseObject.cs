namespace Rassoodock.Databases
{
    public interface ISavableDatabaseObject
    {
        string Text { get; }

        string Schema { get; }

        string Name { get; }
    }
}