using DoggetTelegramBot.Application.Common.Caching;
using DoggetTelegramBot.Application.Helpers;
using DoggetTelegramBot.Application.Users.Common;

namespace DoggetTelegramBot.Application.Users.Queries.Get.Information
{
    public record GetUserInfoByTelegramIdQuery(long TelegramId) :
        ICachedQuery<GetUserInfoResult>
    {
        public string CachedKey => CacheKeyGenerator.GetUserInfoByTelegramId(TelegramId);

        public TimeSpan? Expiration => null;
    }
}
