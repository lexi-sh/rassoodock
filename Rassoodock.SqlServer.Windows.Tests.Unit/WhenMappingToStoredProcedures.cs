using Rassoodock.SqlServer.Windows.Mappings;
using Rassoodock.SqlServer.Windows.Models;
using Shouldly;
using Xunit;

namespace Rassoodock.SqlServer.Windows.Tests.Unit
{
    public class WhenMappingToStoredProcedures
    {
        [Fact]
        public void ShouldNotAppendSchemaWhenAlreadyAppended()
        {
            var sqlServerStoredProc = new RoutinesSqlModel
            {
                definition = "CREATE PROC [abc].[a name]",
                Specific_Name = "a name",
                Specific_Schema = "abc"
            };

            var converter = new RoutineSqlModelTypeConverter();
            var storedProc = converter.Convert(sqlServerStoredProc, null, null);

            storedProc.Text.ShouldContain("CREATE PROC [abc].[a name]");
        }


        [Fact]
        public void ShouldAppendSchemaWhenNotAppendedButInBrackets()
        {
            var sqlServerStoredProc = new RoutinesSqlModel
            {
                definition = "CREATE PROC [a name]",
                Specific_Name = "a name",
                Specific_Schema = "abc"
            };

            var converter = new RoutineSqlModelTypeConverter();
            var storedProc = converter.Convert(sqlServerStoredProc, null, null);

            storedProc.Text.ShouldContain("CREATE PROC [abc].[a name]");
        }

        [Fact]
        public void ShouldAppendSchemaWhenNotAppended()
        {
            var sqlServerStoredProc = new RoutinesSqlModel
            {
                definition = "CREATE PROC aname",
                Specific_Name = "aname",
                Specific_Schema = "abc"
            };

            var converter = new RoutineSqlModelTypeConverter();
            var storedProc = converter.Convert(sqlServerStoredProc, null, null);

            storedProc.Text.ShouldContain("CREATE PROC [abc].[aname]");
        }

        [Fact]
        public void ShouldWrapInBracketsWhenNotIncluded()
        {
            var sqlServerStoredProc = new RoutinesSqlModel
            {
                definition = "CREATE PROC abc.aname",
                Specific_Name = "aname",
                Specific_Schema = "abc"
            };

            var converter = new RoutineSqlModelTypeConverter();
            var storedProc = converter.Convert(sqlServerStoredProc, null, null);

            storedProc.Text.ShouldContain("CREATE PROC [abc].[aname]");
        }

        [Fact]
        public void ShouldWrapInBracketsWhenIncludedInSchemaButNotName()
        {
            var sqlServerStoredProc = new RoutinesSqlModel
            {
                definition = "CREATE PROC [abc].aname",
                Specific_Name = "aname",
                Specific_Schema = "abc"
            };

            var converter = new RoutineSqlModelTypeConverter();
            var storedProc = converter.Convert(sqlServerStoredProc, null, null);

            storedProc.Text.ShouldContain("CREATE PROC [abc].[aname]");
        }
    }
}
