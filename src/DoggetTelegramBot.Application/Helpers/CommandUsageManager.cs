using DoggetTelegramBot.Application.Common.Services;
using ErrorOr;
using DoggetTelegramBot.Domain.Common.Errors;

namespace DoggetTelegramBot.Application.Helpers
{
    public sealed class CommandUsageManager(
        ICacheService cacheService,
        IDateTimeProvider dateTimeProvider)
    {
        public async Task<ErrorOr<bool>> CheckCommandUsageTimeAsync(
            long telegramId,
            string commandName,
            TimeSpan cooldownPeriod,
            CancellationToken cancellationToken)
        {
            string key = CacheKeyGenerator.CommandUsageByTelegramId(telegramId, commandName);

            (bool hasValue, DateTime? cachedTime) = await cacheService
                .TryGetValueAsync<DateTime>(key, cancellationToken);

            if (hasValue)
            {
                var allowedAccessDate = cachedTime.Value.Add(cooldownPeriod);

                return Errors.Permissions.UsageTime(
                    cooldownPeriod,
                    allowedAccessDate);
            }

            return !hasValue;
        }

        public async Task SetCommandUsageTimeAsync(
            long telegramId,
            string commandName,
            TimeSpan cooldownPeriod,
            CancellationToken cancellationToken)
        {
            string key = CacheKeyGenerator.CommandUsageByTelegramId(telegramId, commandName);

            await cacheService.SetUsageTimeAsync(
                key,
                dateTimeProvider.UtcNow.ToLocalTime(),
                cooldownPeriod,
                cancellationToken);
        }
    }
}
