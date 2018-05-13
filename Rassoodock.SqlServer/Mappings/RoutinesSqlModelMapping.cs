using AutoMapper;
using Rassoodock.Common.StartupInterfaces;
using Rassoodock.Databases;
using Rassoodock.SqlServer.Models.Code;
using Rassoodock.SqlServer.Models.SqlModels;

namespace Rassoodock.SqlServer.Mappings
{
    public class RoutinesSqlModelMapping : ICreateMappings
    {
        public void Configure(IMapperConfigurationExpression config)
        {
            config.CreateMap<RoutinesSqlModel, SqlServerStoredProcedure>()
                .ConvertUsing<RoutineSqlModelTypeConverter>();

            config.CreateMap<SqlServerStoredProcedure, StoredProcedure>()
                .ForMember(x => x.Name, c => c.MapFrom(x => x.ObjectName))
                .ForMember(x => x.Schema, c => c.MapFrom(x => x.SchemaName))
                .ForMember(x => x.Text, c => c.ResolveUsing(x => x.GetSourceControlSavableText()));
        }
    }
}
