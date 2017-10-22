using Microsoft.Extensions.CommandLineUtils;
using Rassoodock.Cli;

namespace Rassoodock.Startup
{
    public class CliStatup : IStartup
    {
        public void Startup(string[] args)
        {
            var app = new CommandLineApplication
            {
                Name = "rassoodock"
            };
            app.HelpOption("-?|-h|--help");

            app.Command("link", new LinkCommand().GenerateCommand());
            app.Command("link-config", new LinkConfigCommand().GenerateCommand());

            app.Execute(args);
        }
    }
}
