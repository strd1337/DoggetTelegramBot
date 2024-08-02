namespace DoggetTelegramBot.Application.Common.Services
{
    public interface ICacheService
    {
        Task<T> GetOrCreateAsync<T>(
            string key,
            Func<CancellationToken, Task<T>> factory,
            TimeSpan? expiration = null,
            CancellationToken cancellationToken = default);

        Task RemoveAsync(
            string key,
            CancellationToken cancellationToken = default);

        Task SetUsageTimeAsync(
            string key,
            DateTime usageTime,
            TimeSpan? expiration = null,
            CancellationToken cancellationToken = default);

        Task SetSlidingExpirationAsync<T>(
            string key,
            T value,
            TimeSpan? expiration = null,
            CancellationToken cancellationToken = default);

        Task<(bool hasValue, T? value)> TryGetValueAsync<T>(
            string key,
            CancellationToken cancellationToken = default);
    }
}
