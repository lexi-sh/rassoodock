using AutoMapper;
using Rassoodock.Common.StartupInterfaces;
using Rassoodock.Databases;
using Rassoodock.SqlServer.Models.SqlModels;

namespace Rassoodock.SqlServer.Mappings
{
    public class RoutinesSqlModelMapping : ICreateMappings
    {
        public void Configure(IMapperConfigurationExpression config)
        {
            config.CreateMap<RoutinesSqlModel, StoredProcedure>()
                .ConvertUsing<RoutineSqlModelTypeConverter>();
        }
    }
}
