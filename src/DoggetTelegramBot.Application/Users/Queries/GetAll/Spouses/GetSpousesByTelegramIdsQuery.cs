using DoggetTelegramBot.Application.Common.Caching;
using DoggetTelegramBot.Application.Helpers;
using DoggetTelegramBot.Application.Users.Common;

namespace DoggetTelegramBot.Application.Users.Queries.GetAll.Spouses
{
    public record GetSpousesByTelegramIdsQuery(
        List<long> TelegramIds) : ICachedQuery<GetSpousesResult>
    {
        public string CachedKey => CacheKeyGenerator.GetUsersByTelegramIdsQuery(TelegramIds);

        public TimeSpan? Expiration => null;
    }
}
