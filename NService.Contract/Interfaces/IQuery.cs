using MediatR;

namespace NService.Contract.Interfaces
{
    public interface IQuery<out TResponse>: IRequest<TResponse>
    {
    }
}
