using DoggetTelegramBot.Application.Common.Caching;
using DoggetTelegramBot.Application.Helpers.CacheKeys;
using PRTelegramBot.Models.Enums;

namespace DoggetTelegramBot.Application.Users.Queries.Get.Existence
{
    public record CheckUserExistenceByTelegramIdQuery(
        long TelegramId,
        string? Username,
        string FirstName) : ICachedQuery<UpdateResult>
    {
        public string CachedKey => UserCacheKeyGenerator.UserExistsWithTelegramId(TelegramId);

        public TimeSpan? Expiration => TimeSpan.FromSeconds(10);
    }
}
