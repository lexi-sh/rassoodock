using Rassoodock.SqlServer.Mappings;
using Rassoodock.SqlServer.Models.Domain;
using Shouldly;
using Xunit;

namespace Rassoodock.SqlServer.Tests.Unit
{
    public class WhenMappingToTables
    {
        [Fact]
        public void ShouldSetNotNullableOnColumn()
        {
            var table = new SqlServerTable
            {
                Name = "test1",
                Schema = "dbo",
                Columns = new []
                {
                    new Column
                    {
                        DataType = new DataTypeCls
                        {
                            DataType = DataType.Int
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
                }
            };

            var converter = new TableTypeConverter();
            var tab = converter.Convert(table, null, null);

            tab.Text.ShouldContain("[NotNullable] [int] NOT NULL");
            tab.Text.ShouldContain("[Nullable] [int] NULL");
        }

        [Fact]
        public void ShouldContainTableName()
        {
            var table = new SqlServerTable
            {
                Name = "test1",
                Schema = "dbo2",
                Columns = new[]
                {
                    new Column
                    {
                        DataType = new DataTypeCls
                        {
                            DataType = DataType.Int
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
                }
            };

            var converter = new TableTypeConverter();
            var tab = converter.Convert(table, null, null);

            tab.Text.ShouldContain("CREATE TABLE [dbo2].[test1]");
        }

        [Fact]
        public void ShouldSetDefaultConstraintOnColumn()
        {
            var table = new SqlServerTable
            {
                Name = "test1",
                Schema = "dbo",
                Columns = new[]
                {
                    new Column
                    {
                        DataType = new DataTypeCls
                        {
                            DataType = DataType.Bit
                        },
                        Name = "NotNullable",
                        Nullable = false,
                        DefaultConstraint = new DefaultConstraint
                        {
                            Name = "df_default",
                            DefaultVlaue = "(0)"
                        }
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
                }
            };

            var converter = new TableTypeConverter();
            var tab = converter.Convert(table, null, null);

            tab.Text.ShouldContain("[NotNullable] [bit] NOT NULL CONSTRAINT [df_default] DEFAULT ((0))");
            tab.Text.ShouldContain("[Nullable] [int] NULL");
        }

        [Fact]
        public void ShouldNotHaveTextImageIfNoBigTextColumn()
        {
            var table = new SqlServerTable
            {
                Name = "test1",
                Schema = "dbo2",
                Columns = new[]
                {
                    new Column
                    {
                        DataType = new DataTypeCls
                        {
                            DataType = DataType.Int
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
                }
            };

            var converter = new TableTypeConverter();
            var tab = converter.Convert(table, null, null);

            tab.Text.ShouldNotContain("TEXTIMAGE_ON [PRIMARY]");
        }

        [Fact]
        public void ShouldHaveTextImageIfBigTextColumn()
        {
            var table = new SqlServerTable
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
                }
            };

            var converter = new TableTypeConverter();
            var tab = converter.Convert(table, null, null);

            tab.Text.ShouldContain("TEXTIMAGE_ON [PRIMARY]");
        }

        [Fact]
        public void ShouldHaveTriggers()
        {
            var table = new SqlServerTable
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
                Triggers = new [] { new TableTrigger
                {
                  Schema  = "dbo",
                  Name = "trig1",
                  Action = TriggerAction.Insert,
                  TableName = "test1",
                  TableSchema = "dbo2",
                  Text = "SELECT 1233"
                }}
            };

            var converter = new TableTypeConverter();
            var tab = converter.Convert(table, null, null);

            tab.Text.ShouldContain("SELECT 1233");

        }

        [Fact]
        public void ShouldHavePrimaryKeyConstraint()
        {
            var table = new SqlServerTable
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
                }
            };

            table.PrimaryKeyConstraint = new PrimaryKeyConstraint
            {
                Clustered = true,
                Name = "pk",
                Columns = table.Columns
            };

            var converter = new TableTypeConverter();
            var tab = converter.Convert(table, null, null);

            tab.Text.ShouldContain("ALTER TABLE [dbo2].[test1] ADD CONSTRAINT [pk] PRIMARY KEY CLUSTERED  ([NotNullable], [Nullable]) ON [PRIMARY]");

        }
    }
}
