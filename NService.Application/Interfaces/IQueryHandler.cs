using MediatR;
using NService.Contract.Interfaces;

namespace NService.Application.Interfaces
{
    public interface IQueryHandler<in TRequest, TResponse> : IAsyncRequestHandler<TRequest, TResponse>
        where TRequest: IQuery<TResponse>
    {
    }
}
