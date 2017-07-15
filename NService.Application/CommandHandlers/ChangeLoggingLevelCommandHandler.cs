using System;
using System.Threading.Tasks;
using NService.Application.Interfaces;
using NService.Contract.Commands;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace NService.Application.CommandHandlers
{
    public class ChangeLoggingLevelCommandHandler : ICommandHandler<ChangeLoggingLevelCommand>
    {
        private readonly LoggingLevelSwitch _loggingLevelSwitch;
        private readonly ILogger _logger;

        public ChangeLoggingLevelCommandHandler(LoggingLevelSwitch loggingLevelSwitch, ILogger logger)
        {
            _logger = logger.ForContext<ChangeLoggingLevelCommandHandler>();
            _loggingLevelSwitch = loggingLevelSwitch;
        }

        public Task Handle(ChangeLoggingLevelCommand message)
        {
            _loggingLevelSwitch.MinimumLevel = message.MinimumLevel;
            _logger.Information("The minimum logging level was changed to @LoggingLevel", new { LoggingLevel = Enum.GetName(typeof(LogEventLevel), message.MinimumLevel) });
            return Task.CompletedTask;
        }
    }
}
