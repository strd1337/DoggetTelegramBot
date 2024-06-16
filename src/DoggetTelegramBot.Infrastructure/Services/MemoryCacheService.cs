using DoggetTelegramBot.Application.Common.Services;
using Microsoft.Extensions.Caching.Memory;

namespace DoggetTelegramBot.Infrastructure.Services
{
    public class MemoryCacheService(IMemoryCache memoryCache) : ICacheService
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

            return result!;
        }
    }
}
