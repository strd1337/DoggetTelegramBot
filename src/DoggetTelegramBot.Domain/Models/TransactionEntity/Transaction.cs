using DoggetTelegramBot.Domain.Common.Entities;
using DoggetTelegramBot.Domain.Models.ItemEntity;
using DoggetTelegramBot.Domain.Models.TransactionEntity.Enums;
using DoggetTelegramBot.Domain.Models.UserEntity;

namespace DoggetTelegramBot.Domain.Models.TransactionEntity
{
    public sealed class Transaction : Root<TransactionId, Guid>
    {
        private readonly List<ItemId> itemIds = [];

        public UserId FromUserId { get; private set; }
        public UserId ToUserId { get; private set; }
        public decimal? Amount { get; private set; }
        public TransactionType Type { get; private set; }
        public IReadOnlyList<ItemId> ItemIds
            => itemIds.AsReadOnly();

        private Transaction(
            TransactionId transactionId,
            UserId fromUserId,
            UserId toUserId,
            TransactionType type,
            decimal? amount = null) : base(transactionId)
        {
            FromUserId = fromUserId;
            ToUserId = toUserId;
            Type = type;
            Amount = amount;
        }

        public static Transaction Create(
            UserId fromUserId,
            UserId toUserId,
            TransactionType type,
            decimal? amount = null) => new(
                TransactionId.CreateUnique(),
                fromUserId,
                toUserId,
                type,
                amount);

#pragma warning disable CS8618
        private Transaction()
        {
        }
#pragma warning restore CS8618
    }
}
