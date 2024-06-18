using DoggetTelegramBot.Application.Common.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace DoggetTelegramBot.Infrastructure.Services
{
    public class ScopeService(IServiceScopeFactory serviceScopeFactory)
        : IScopeService
    {
        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            return await mediator.Send(request);
        }
    }
}
