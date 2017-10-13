using AutoMapper;

namespace Rassoodock.Common.StartupInterfaces
{
    public interface ICreateMappings
    {
        void Configure(IMapperConfigurationExpression cfg);
    }
}