using DoggetTelegramBot.Application.Common.Caching;
using DoggetTelegramBot.Application.Users.Common;

namespace DoggetTelegramBot.Application.Users.Queries.Get.Information
{
    public record GetUserInfoQuery(long TelegramId) :
        ICachedQuery<GetUserInfoResult>
    {
        public string CachedKey => $"get-user-info-by-telegramId-{TelegramId}";

        public TimeSpan? Expiration => null;
    }
}
