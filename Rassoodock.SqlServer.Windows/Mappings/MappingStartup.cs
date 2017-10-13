using AutoMapper;
using Rassoodock.Common.StartupInterfaces;

namespace Rassoodock.SqlServer.Windows.Mappings
{
    public class MappingStartup : IConfigureAutomapper
    {
        public void Configure(IMapperConfigurationExpression config)
        {
            var mappers = new ICreateMappings[]
            {
                new RoutinesSqlModelMapping()
            };

            foreach (var mapper in mappers)
            {
                mapper.Configure(config);
            }
        }
    }
}
