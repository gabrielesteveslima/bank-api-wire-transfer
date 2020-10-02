using MediatR;

namespace Wire.Transfer.In.Application
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {
    }
}