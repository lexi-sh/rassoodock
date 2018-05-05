using System;
using Xunit;
using Rassoodock.SqlServer.Models.Domain;
using Rassoodock.DifferenceEngine.SqlServer.SqlServerTableStrategies;
using Shouldly;

namespace Rassoodock.DifferenceEngine.SqlServer.Tests.Unit
{
    public class SqlServerColumnAdditionDifferentiationStrategyTests
    {
        [Fact]
        public void ColumnAdditionStrategyOutputsStringsForMissingColumns()
        {
            var liveTable = new SqlServerTable
            {
                Name = "test1",
                Schema = "dbo2",
                Columns = new[]
                {
                    new Column
                    {
                        DataType = new DataTypeCls
                        {
                            DataType = DataType.NVarChar,
                            Length = SqlServerConstants.MaxLength
                        },
                        Name = "NotNullable",
                        Nullable = false
                    }
                },
                Triggers = new [] 
                { 
                    new TableTrigger
                    {
                        Schema  = "dbo",
                        Name = "trig1",
                        Action = TriggerAction.Insert,
                        TableName = "test1",
                        TableSchema = "dbo2",
                        Text = "SELECT 1233"
                    }
                }
            };

            var scTable = new SqlServerTable
            {
                Name = "test1",
                Schema = "dbo2",
                Columns = new[]
                {
                    new Column
                    {
                        DataType = new DataTypeCls
                        {
                            DataType = DataType.NVarChar,
                            Length = SqlServerConstants.MaxLength
                        },
                        Name = "NotNullable",
                        Nullable = false
                    },
                    new Column
                    {
                        DataType = new DataTypeCls
                        {
                            DataType = DataType.Int
                        },
                        Name = "Nullable",
                        Nullable = true
                    }
                },
                Triggers = new [] 
                { 
                    new TableTrigger
                    {
                        Schema  = "dbo",
                        Name = "trig1",
                        Action = TriggerAction.Insert,
                        TableName = "test1",
                        TableSchema = "dbo2",
                        Text = "SELECT 1233"
                    }
                }
            };
            var strategy = new SqlServerColumnAdditionDifferentiationStrategy(scTable, liveTable);
            strategy.GetDifferenceAlterString().ShouldContain("ALTER TABLE [dbo2].[test1] ADD");
            strategy.GetDifferenceAlterString().ShouldContain("[Nullable] [int] NULL;");
        }
    }
}
