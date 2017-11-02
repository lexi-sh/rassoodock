using AutoMapper;
using Rassoodock.Common.StartupInterfaces;
using Rassoodock.SqlServer.Mappings;

namespace Rassoodock.Startup
{
    public class AutomapperStartup : IStartup
    {
        public void Startup(string[] args)
        {
            var startups = new IConfigureAutomapper[]
            {
                new MappingStartup()
            };
            Mapper.Initialize(cfg =>
            {
                foreach (var startup in startups)
                {
                    startup.Configure(cfg);
                }
            });
        }
    }
}
