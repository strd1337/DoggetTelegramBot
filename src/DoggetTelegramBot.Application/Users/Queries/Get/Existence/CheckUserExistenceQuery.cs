using DoggetTelegramBot.Application.Common.Caching;
using PRTelegramBot.Models.Enums;

namespace DoggetTelegramBot.Application.Users.Queries.Get.Existence
{
    public record CheckUserExistenceQuery(
        long TelegramId,
        string? Username,
        string? FirstName,
        string? LastName) : ICachedQuery<ResultUpdate>
    {
        public string CachedKey => $"user-exists-with-telegramId-{TelegramId}";

        public TimeSpan? Expiration => TimeSpan.FromHours(1);
    }
}
