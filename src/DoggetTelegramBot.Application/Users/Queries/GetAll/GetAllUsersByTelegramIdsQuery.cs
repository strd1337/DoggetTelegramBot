using DoggetTelegramBot.Application.Common.Caching;
using DoggetTelegramBot.Application.Helpers;
using DoggetTelegramBot.Application.Users.Common;

namespace DoggetTelegramBot.Application.Users.Queries.GetAll
{
    public record GetAllUsersByTelegramIdsQuery(
        List<long> TelegramIds) : ICachedQuery<GetAllUsersResult>
    {
        public string CachedKey => CacheKeyGenerator.GetAllUsersByTelegramIds(TelegramIds);

        public TimeSpan? Expiration => null;
    }
}
