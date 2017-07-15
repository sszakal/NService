using MediatR;
using NService.Contract.Interfaces;

namespace NService.Application.Interfaces
{
    public interface ICommandHandler<in TRequest>: IAsyncRequestHandler<TRequest>
        where TRequest: ICommand
    {
    }
}
