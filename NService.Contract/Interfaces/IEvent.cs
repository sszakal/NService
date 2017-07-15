using System;
using MediatR;

namespace NService.Contract.Interfaces
{
    public interface IEvent: INotification
    {
        Guid CorrelationId { get; }
    }
}
