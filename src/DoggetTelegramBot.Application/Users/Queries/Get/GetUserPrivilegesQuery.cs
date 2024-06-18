using DoggetTelegramBot.Application.Common.Caching;
using DoggetTelegramBot.Application.Users.Queries.Common;

namespace DoggetTelegramBot.Application.Users.Queries.Get
{
    public record GetUserPrivilegesQuery(long TelegramId) :
        ICachedQuery<GetUserPrivilegesResult>
    {
        public string CachedKey => $"get-user-privileges-by-telegramId-{TelegramId}";

        public TimeSpan? Expiration => null;
    }
}
