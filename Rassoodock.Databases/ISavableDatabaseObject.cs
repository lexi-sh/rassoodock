namespace Rassoodock.Databases
{
    public abstract class SavableDatabaseObject
    {
        public string Text { get; set; }

        public string Schema { get; set; }

        public string Name { get; set; }
    }
}