namespace Rassoodock.Databases
{
    public class StoredProcedure : ISavableDatabaseObject
    {
        public string Text { get; set; }

        public string Schema { get; set; }

        public string Name { get; set; }
    }
}
