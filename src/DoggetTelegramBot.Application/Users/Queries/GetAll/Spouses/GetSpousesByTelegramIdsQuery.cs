using DoggetTelegramBot.Application.Common.Caching;
using DoggetTelegramBot.Application.Helpers;
using DoggetTelegramBot.Application.Users.Common;

namespace DoggetTelegramBot.Application.Users.Queries.GetAll.Spouses
{
    public record GetSpousesByTelegramIdsQuery(
        List<long> TelegramIds,
        bool IsGetMarried = true) : ICachedQuery<GetSpousesResult>
    {
        public string CachedKey => CacheKeyGenerator.GetSpousesByTelegramIdsQuery(TelegramIds);

        public TimeSpan? Expiration => null;
    }
}
