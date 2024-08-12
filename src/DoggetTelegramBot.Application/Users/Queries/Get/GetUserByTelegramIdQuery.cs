using DoggetTelegramBot.Application.Common.Caching;
using DoggetTelegramBot.Application.Helpers.CacheKeys;
using DoggetTelegramBot.Application.Users.Common;

namespace DoggetTelegramBot.Application.Users.Queries.Get
{
    public record GetUserByTelegramIdQuery(
        long TelegramId) : ICachedQuery<GetUserResult>
    {
        public string CachedKey => UserCacheKeyGenerator.GetUserByTelegramId(TelegramId);

        public TimeSpan? Expiration => null;
    }
}
