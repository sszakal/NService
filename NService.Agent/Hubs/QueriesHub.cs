using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNet.SignalR;
using NService.Contract.Queries;

namespace NService.Agent.Hubs
{
    public class QueriesHub : Hub
    {
        private readonly IMediator _mediator;

        public QueriesHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<LoggingLevelQuery.LoggingLevelQueryResponse> Handle(LoggingLevelQuery query)
        {
            return await _mediator.Send(query);
        }
    }
}
