using MediatR;
using NService.Contract.Interfaces;

namespace NService.Application.Interfaces
{
    public interface IEventHandler<in TRequest>: IAsyncNotificationHandler<TRequest>
        where TRequest: IEvent
    {
    }
}
