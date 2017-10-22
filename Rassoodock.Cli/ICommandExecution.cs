using System;
using Microsoft.Extensions.CommandLineUtils;

namespace Rassoodock.Cli
{
    public interface ICommandExecution
    {
        Action<CommandLineApplication> GenerateCommand();
    }
}