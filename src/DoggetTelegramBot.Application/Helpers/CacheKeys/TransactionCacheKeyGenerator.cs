using DoggetTelegramBot.Domain.Models.TransactionEntity;

namespace DoggetTelegramBot.Application.Helpers.CacheKeys
{
    public static class TransactionCacheKeyGenerator
    {
        private const string ModelName = nameof(Transaction);

        public static string GetTransactionParticipantsByTelegramIdsQuery(long? fromTelegramId, long toTelegramId) =>
            CacheKeyGenerator.GenerateKey(ModelName, nameof(GetTransactionParticipantsByTelegramIdsQuery),
                fromTelegramId is null ? $"{toTelegramId}" : $"{fromTelegramId},{toTelegramId}");
    }
}
