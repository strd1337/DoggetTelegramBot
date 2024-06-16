using MediatR;

namespace DoggetTelegramBot.Application.Common.Services
{
    public interface IScopedMediatorService
    {
        Task<TResponse> Send<TResponse>(IRequest<TResponse> request);
    }
}
