using AutoMapper;
using Rassoodock.Common.StartupInterfaces;
using Rassoodock.Databases;
using Rassoodock.SqlServer.Models.Code;
using Rassoodock.SqlServer.Models.Domain;
using Rassoodock.SqlServer.Models.SqlModels;

namespace Rassoodock.SqlServer.Mappings
{
    public class TableSqlModelMapping : ICreateMappings
    {
        public void Configure(IMapperConfigurationExpression config)
        {
            config.CreateMap<SqlServerTable, Table>()
                .ConvertUsing<TableTypeConverter>();

            config.CreateMap<TableSqlModel, SqlServerTable>();
        }
    }
}
