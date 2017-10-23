using Rassoodock.Startup;

namespace Rassoodock
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var startups = new IStartup[]
            {
                new AutomapperStartup(),
                new CliStatup()
            };

            foreach (var startup in startups)
            {
                startup.Startup(args);
            }
        }
    }
}
