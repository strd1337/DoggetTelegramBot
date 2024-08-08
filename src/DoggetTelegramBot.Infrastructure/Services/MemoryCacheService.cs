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
            if (memoryCache.TryGetValue(key, out _))
            {
                memoryCache.Remove(key);

                logger.LogCommon(
                    Constants.Cache.RemoveMessage,
                    TelegramEvents.Message,
                    Constants.LogColors.Cache);
            }

            return Task.CompletedTask;
        }

        public Task SetSlidingExpirationAsync<T>(
            string key,
            T value,
            TimeSpan? expiration = null,
            CancellationToken cancellationToken = default)
        {
            MemoryCacheEntryOptions cacheEntryOptions = new()
            {
                SlidingExpiration = expiration ?? defaultExpiration
            };

            memoryCache.Set(key, value, cacheEntryOptions);

            logger.LogCommon(
                Constants.Cache.StoreOrRetrieveMessage,
                TelegramEvents.Message,
                Constants.LogColors.Cache);

            return Task.CompletedTask;
        }

        public Task SetUsageTimeAsync(
            string key,
            DateTime usageTime,
            TimeSpan? expiration = null,
            CancellationToken cancellationToken = default)
        {
            memoryCache.Set(key, usageTime, expiration ?? defaultExpiration);

            logger.LogCommon(
                Constants.Cache.StoreOrRetrieveMessage,
                TelegramEvents.Message,
                Constants.LogColors.Cache);

            return Task.CompletedTask;
        }

        public Task<(bool hasValue, T? value)> TryGetValueAsync<T>(
            string key,
            CancellationToken cancellationToken = default)
        {
            bool hasValue = memoryCache.TryGetValue(key, out T? value);
            return Task.FromResult((hasValue, value));
        }
    }
}
