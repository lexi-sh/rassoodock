using Rassoodock.SqlServer.Windows.Models;
using Shouldly;
using Xunit;

namespace Rassoodock.SqlServer.Windows.Tests
{
    public class WhenMappingToStoredProcedures
    {
        [Fact]
        public void ShouldNotAppendSchemaWhenAlreadyAppended()
        {
            var sqlServerStoredProc = new RoutinesSqlModel
            {
                Routine_Definition = "CREATE PROC [abc].[a name]",
                Specific_Name = "a name",
                Specific_Schema = "abc"
            };

            var storedProc = sqlServerStoredProc.MapToStoredProcedure();

            storedProc.Text.ShouldBe("CREATE PROC [abc].[a name]");
        }


        [Fact]
        public void ShouldAppendSchemaWhenNotAppendedButInBrackets()
        {
            var sqlServerStoredProc = new RoutinesSqlModel
            {
                Routine_Definition = "CREATE PROC [a name]",
                Specific_Name = "a name",
                Specific_Schema = "abc"
            };

            var storedProc = sqlServerStoredProc.MapToStoredProcedure();

            storedProc.Text.ShouldBe("CREATE PROC [abc].[a name]");
        }

        [Fact]
        public void ShouldAppendSchemaWhenNotAppended()
        {
            var sqlServerStoredProc = new RoutinesSqlModel
            {
                Routine_Definition = "CREATE PROC aname",
                Specific_Name = "aname",
                Specific_Schema = "abc"
            };

            var storedProc = sqlServerStoredProc.MapToStoredProcedure();

            storedProc.Text.ShouldBe("CREATE PROC [abc].[aname]");
        }

        [Fact]
        public void ShouldWrapInBracketsWhenNotIncluded()
        {
            var sqlServerStoredProc = new RoutinesSqlModel
            {
                Routine_Definition = "CREATE PROC abc.aname",
                Specific_Name = "aname",
                Specific_Schema = "abc"
            };

            var storedProc = sqlServerStoredProc.MapToStoredProcedure();

            storedProc.Text.ShouldBe("CREATE PROC [abc].[aname]");
        }

        [Fact]
        public void ShouldWrapInBracketsWhenIncludedInSchemaButNotName()
        {
            var sqlServerStoredProc = new RoutinesSqlModel
            {
                Routine_Definition = "CREATE PROC [abc].aname",
                Specific_Name = "aname",
                Specific_Schema = "abc"
            };

            var storedProc = sqlServerStoredProc.MapToStoredProcedure();

            storedProc.Text.ShouldBe("CREATE PROC [abc].[aname]");
        }
    }
}
