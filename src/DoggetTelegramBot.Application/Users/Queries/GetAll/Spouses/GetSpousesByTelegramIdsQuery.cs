using DoggetTelegramBot.Application.Common.Caching;
using DoggetTelegramBot.Application.Helpers.CacheKeys;
using DoggetTelegramBot.Application.Users.Common;

namespace DoggetTelegramBot.Application.Users.Queries.GetAll.Spouses
{
    public record GetSpousesByTelegramIdsQuery(
        List<long> TelegramIds,
        bool IsGetMarried = true) : ICachedQuery<GetSpousesResult>
    {
        public string CachedKey => UserCacheKeyGenerator.GetSpousesByTelegramIdsQuery(TelegramIds);

        public TimeSpan? Expiration => null;
    }
}
