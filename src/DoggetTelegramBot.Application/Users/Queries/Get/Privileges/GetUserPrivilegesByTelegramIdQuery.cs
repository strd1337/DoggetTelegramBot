using DoggetTelegramBot.Application.Common.Caching;
using DoggetTelegramBot.Application.Helpers;
using DoggetTelegramBot.Application.Users.Common;

namespace DoggetTelegramBot.Application.Users.Queries.Get.Privileges
{
    public record GetUserPrivilegesByTelegramIdQuery(long TelegramId) :
        ICachedQuery<GetUserPrivilegesResult>
    {
        public string CachedKey => CacheKeyGenerator.GetUserPrivilegesByTelegramId(TelegramId);

        public TimeSpan? Expiration => null;
    }
}
