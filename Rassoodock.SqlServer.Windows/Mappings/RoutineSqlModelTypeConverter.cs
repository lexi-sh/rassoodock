using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using AutoMapper;
using Rassoodock.Databases;
using Rassoodock.SqlServer.Windows.Models;

namespace Rassoodock.SqlServer.Windows.Mappings
{
    public class RoutineSqlModelTypeConverter : ITypeConverter<RoutinesSqlModel, StoredProcedure>
    {
        public StoredProcedure Convert(RoutinesSqlModel source, StoredProcedure destination, ResolutionContext context)
        {
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

            var regex = new Regex($@"\[?({Regex.Escape(source.Specific_Schema)})?\]?\.?\[?{Regex.Escape(source.Specific_Name)}\]?");

            var newName = $"[{source.Specific_Schema}].[{source.Specific_Name}]";

            return new StoredProcedure
            {
                Name = source.Specific_Name,
                Schema = source.Specific_Schema,
                Text = regex.Replace(source.Routine_Definition, newName, 1)
            };
        }
    }
}
