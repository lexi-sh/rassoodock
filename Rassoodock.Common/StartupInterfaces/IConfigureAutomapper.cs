using AutoMapper;

namespace Rassoodock.Common.StartupInterfaces
{
    public interface IConfigureAutomapper
    {
        void Configure(IMapperConfigurationExpression config);
    }
}