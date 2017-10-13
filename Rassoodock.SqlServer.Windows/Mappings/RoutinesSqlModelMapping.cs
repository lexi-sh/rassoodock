using AutoMapper;
using Rassoodock.Common.StartupInterfaces;
using Rassoodock.Databases;
using Rassoodock.SqlServer.Windows.Models;

namespace Rassoodock.SqlServer.Windows.Mappings
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
