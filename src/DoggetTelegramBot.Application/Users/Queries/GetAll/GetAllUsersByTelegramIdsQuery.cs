using DoggetTelegramBot.Application.Common.Caching;
using DoggetTelegramBot.Application.Helpers.CacheKeys;
using DoggetTelegramBot.Application.Users.Common;

namespace DoggetTelegramBot.Application.Users.Queries.GetAll
{
    public record GetAllUsersByTelegramIdsQuery(
        List<long> TelegramIds) : ICachedQuery<GetAllUsersResult>
    {
        public string CachedKey => UserCacheKeyGenerator.GetAllUsersByTelegramIds(TelegramIds);

        public TimeSpan? Expiration => null;
    }
}
