using System;
using MediatR;
using TypeLite;

namespace NService.Contract.Interfaces
{
    public interface ICommand: IRequest
    {
        Guid CorrelationId { get; }
    }
}
