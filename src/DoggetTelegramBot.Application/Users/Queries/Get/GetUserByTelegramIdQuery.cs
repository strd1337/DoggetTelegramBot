using DoggetTelegramBot.Application.Common.Caching;
using DoggetTelegramBot.Application.Helpers;
using DoggetTelegramBot.Application.Users.Common;

namespace DoggetTelegramBot.Application.Users.Queries.Get
{
    public record GetUserByTelegramIdQuery(
        long TelegramId) : ICachedQuery<GetUserResult>
    {
        public string CachedKey => CacheKeyGenerator.GetUserByTelegramId(TelegramId);

        public TimeSpan? Expiration => null;
    }
}
