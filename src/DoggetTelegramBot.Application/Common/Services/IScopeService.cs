using MediatR;

namespace DoggetTelegramBot.Application.Common.Services
{
    public interface IScopeService
    {
        Task<TResponse> Send<TResponse>(IRequest<TResponse> request);
    }
}
