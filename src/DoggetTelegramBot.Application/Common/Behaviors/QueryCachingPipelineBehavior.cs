using DoggetTelegramBot.Application.Common.Caching;
using DoggetTelegramBot.Application.Common.Services;
using MediatR;

namespace DoggetTelegramBot.Application.Common.Behaviors
{
    internal sealed class QueryCachingPipelineBehavior<TRequest, TResponse>(
        ICacheService cacheService)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICachedQuery
    {
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken) => await cacheService.GetOrCreateAsync(
                request.CachedKey,
                _ => next(),
                request.Expiration,
                cancellationToken);
    }
}
