using DoggetTelegramBot.Application.Common.Caching;
using DoggetTelegramBot.Application.Helpers;
using PRTelegramBot.Models.Enums;

namespace DoggetTelegramBot.Application.Users.Queries.Get.Existence
{
    public record CheckUserExistenceByTelegramIdQuery(
        long TelegramId,
        string? Username) : ICachedQuery<UpdateResult>
    {
        public string CachedKey => CacheKeyGenerator.UserExistsWithTelegramId(TelegramId);

        public TimeSpan? Expiration => TimeSpan.FromHours(1);
    }
}
