using System.Diagnostics;
using System.Threading.Tasks;
using MediatR;
using NService.Application;
using Serilog;
using Serilog.Context;

namespace NService.Agent.Mediatr
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger _logger;

        public LoggingBehavior(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next)
        {
            _logger.Information("Started Executing {@Message}.", new { Message = typeof(TRequest).Name });

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var response = await next();

            stopWatch.Stop();

            _logger.Information("Finished Executing {@Message} in {@ElapsedMilliseconds}.", new { Message = typeof(TRequest).Name, ElapsedMilliseconds = stopWatch.ElapsedMilliseconds });
            return response;
        }
    }
}
