using ErrorOr;
using MediatR;

namespace DoggetTelegramBot.Application.Common.CQRS
{
    public interface IQuery<TResponse> : IRequest<ErrorOr<TResponse>>
    {
    }
}
