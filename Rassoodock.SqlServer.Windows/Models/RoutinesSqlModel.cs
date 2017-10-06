using Rassoodock.Databases;

namespace Rassoodock.SqlServer.Windows.Models
{
    public class RoutinesSqlModel
    {
        public string Routine_Definition { get; set; }

        public string Specific_Schema { get; set; }

        public string Specific_Name { get; set; }

        public StoredProcedure MapToStoredProcedure()
        {
            // TODO: Implement AutoMapper

            

            return new StoredProcedure
            {
                Name = Specific_Name,
                Schema = Specific_Schema,

            };
        }
    }
}
