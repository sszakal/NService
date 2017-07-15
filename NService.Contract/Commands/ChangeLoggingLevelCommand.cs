using Serilog.Events;
using TypeLite;

namespace NService.Contract.Commands
{
    [TsClass]
    public class ChangeLoggingLevelCommand: CommandBase
    {
        public LogEventLevel MinimumLevel { get; set; }
    }
}
