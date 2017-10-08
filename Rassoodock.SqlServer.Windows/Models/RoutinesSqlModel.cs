using System.Text.RegularExpressions;
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
            
            /* In SqlServer, the routine definition looks like this:
             *  comments
             *  CREATE PROC[EDURE]
             *  NAME
             *  ...
             *  
             *  The name is whatever is defined by the user, so maybe brackets,
             *  maybe not... maybe schema, maybe not. 
             *  But we need to standardize on always brackets, always add schema
             *
             * NAME could be any of the following formats:
             * [sch].[na me]
             * sch.[na me]
             * sch.name
             * [sch].name
             * name
             * [na me]
             */

            var regex = new Regex($@"\[?({Regex.Escape(Specific_Schema)})?\]?\.?\[?{Regex.Escape(Specific_Name)}\]?");
            
            var newName = $"[{Specific_Schema}].[{Specific_Name}]";

            return new StoredProcedure
            {
                Name = Specific_Name,
                Schema = Specific_Schema,
                Text = regex.Replace(Routine_Definition, newName, 1)
            };
        }
    }
}
