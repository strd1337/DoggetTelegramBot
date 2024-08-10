using DoggetTelegramBot.Application.Common.Caching;
using DoggetTelegramBot.Application.Helpers.CacheKeys;
using DoggetTelegramBot.Application.Users.Common;

namespace DoggetTelegramBot.Application.Users.Queries.Get.Information
{
    public record GetUserInfoByTelegramIdQuery(long TelegramId) :
        ICachedQuery<GetUserInfoResult>
    {
        public string CachedKey => UserCacheKeyGenerator.GetUserInfoByTelegramId(TelegramId);

        public TimeSpan? Expiration => null;
    }
}
