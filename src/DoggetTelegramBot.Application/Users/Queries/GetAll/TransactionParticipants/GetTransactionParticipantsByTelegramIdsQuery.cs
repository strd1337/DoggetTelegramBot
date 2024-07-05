using DoggetTelegramBot.Application.Common.Caching;
using DoggetTelegramBot.Application.Helpers;
using DoggetTelegramBot.Application.Users.Common;

namespace DoggetTelegramBot.Application.Users.Queries.GetAll.TransactionParticipants
{
    public record GetTransactionParticipantsByTelegramIdsQuery(long ToTelegramId, long? FromTelegramId = null)
        : ICachedQuery<GetTransactionParticipantsResult>
    {
        public string CachedKey => CacheKeyGenerator
            .GetTransactionParticipantsByTelegramIdsQuery(FromTelegramId, ToTelegramId);

        public TimeSpan? Expiration => null;
    }
}
