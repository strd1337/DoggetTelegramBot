using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using Microsoft.Extensions.Caching.Memory;

namespace DoggetTelegramBot.Infrastructure.Services
{
    public class MemoryCacheService(
        IMemoryCache memoryCache,
        IBotLogger logger) : ICacheService
    {
        private readonly TimeSpan defaultExpiration = TimeSpan.FromMinutes(5);

        public async Task<T> GetOrCreateAsync<T>(
            string key,
            Func<CancellationToken, Task<T>> factory,
            TimeSpan? expiration = null,
            CancellationToken cancellationToken = default)
        {
            var result = await memoryCache.GetOrCreateAsync(
                key,
                entry =>
                {
                    entry.SetAbsoluteExpiration(expiration ?? defaultExpiration);
                    return factory(cancellationToken);
                });

            logger.LogCommon(
                Constants.Cache.StoreOrRetrieveMessage,
                TelegramEvents.Message,
                Constants.LogColors.Cache);

            return result!;
        }

        public Task RemoveAsync(
            string key,
            CancellationToken cancellationToken = default)
        {
            bool isRetrieved = memoryCache.TryGetValue(key, out _);

            if (isRetrieved)
            {
                memoryCache.Remove(key);

                logger.LogCommon(
                    Constants.Cache.RemoveMessage,
                    TelegramEvents.Message,
                    Constants.LogColors.Cache);
        }

            return Task.CompletedTask;
        }
    }
}
