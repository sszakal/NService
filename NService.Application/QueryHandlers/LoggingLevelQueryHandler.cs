using System;
using System.Threading.Tasks;
using NService.Application.Interfaces;
using NService.Contract.Queries;
using Serilog.Core;
using Serilog.Events;

namespace NService.Application.QueryHandlers
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class LoggingLevelQueryHandler : IQueryHandler<LoggingLevelQuery, LoggingLevelQuery.LoggingLevelQueryResponse>
    {
        private readonly LoggingLevelSwitch _loggingLevelSwitch;

        public LoggingLevelQueryHandler(LoggingLevelSwitch loggingLevelSwitch)
        {
            _loggingLevelSwitch = loggingLevelSwitch;
        }

        public Task<LoggingLevelQuery.LoggingLevelQueryResponse> Handle(LoggingLevelQuery message)
        {
            return Task.FromResult(new LoggingLevelQuery.LoggingLevelQueryResponse((int)_loggingLevelSwitch.MinimumLevel, 
                                                                                   Enum.GetName(typeof(LogEventLevel), _loggingLevelSwitch.MinimumLevel)));
        }
    }
}
