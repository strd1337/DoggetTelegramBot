using DoggetTelegramBot.Domain.Common.Entities;
using DoggetTelegramBot.Domain.Models.ItemEntity;
using DoggetTelegramBot.Domain.Models.TransactionEntity.Enums;
using DoggetTelegramBot.Domain.Models.UserEntity;

namespace DoggetTelegramBot.Domain.Models.TransactionEntity
{
    public sealed class Transaction : Entity
    {
        private readonly List<ItemId> itemIds = [];

        public TransactionId TransactionId { get; private set; }
        public List<UserId> FromUserIds { get; private set; } = [];
        public List<UserId> ToUserIds { get; private set; } = [];
        public decimal? Amount { get; private set; }
        public TransactionType Type { get; private set; }
        public IReadOnlyList<ItemId> ItemIds
            => itemIds.AsReadOnly();

        public static Transaction CreateServiceFee(
            List<UserId> fromUserIds,
            decimal amount) => new()
            {
                TransactionId = TransactionId.CreateUnique(),
                FromUserIds = fromUserIds,
                Amount = amount,
                Type = TransactionType.ServiceFee
            };

        public static Transaction CreateTransfer(
            UserId fromUserId,
            UserId toUserId,
            List<ItemId> items,
            decimal? amount = null)
        {
            Transaction transaction = new()
            {
                TransactionId = TransactionId.CreateUnique(),
                FromUserIds = [fromUserId],
                ToUserIds = [toUserId],
                Amount = amount,
                Type = TransactionType.Transfer
            };
            transaction.itemIds.AddRange(items);
            return transaction;
        }

        public static Transaction CreatePurchase(
            UserId toUserId,
            decimal amount,
            List<ItemId> items)
        {
            Transaction transaction = new()
            {
                TransactionId = TransactionId.CreateUnique(),
                ToUserIds = [toUserId],
                Amount = amount,
                Type = TransactionType.Purchase
            };
            transaction.itemIds.AddRange(items);
            return transaction;
        }

        public static Transaction CreateReward(
            UserId fromUserId,
            UserId toUserId,
            List<ItemId> items,
            decimal? amount = null)
        {
            Transaction transaction = new()
            {
                TransactionId = TransactionId.CreateUnique(),
                FromUserIds = [fromUserId],
                ToUserIds = [toUserId],
                Amount = amount,
                Type = TransactionType.Reward
            };
            transaction.itemIds.AddRange(items);
            return transaction;
        }

        public static Transaction CreatePenalty(
            UserId fromUserId,
            UserId toUserId,
            List<ItemId> items,
            decimal? amount = null)
        {
            Transaction transaction = new()
            {
                TransactionId = TransactionId.CreateUnique(),
                FromUserIds = [fromUserId],
                ToUserIds = [toUserId],
                Amount = amount,
                Type = TransactionType.Penalty
            };
            transaction.itemIds.AddRange(items);
            return transaction;
        }

#pragma warning disable CS8618
        private Transaction()
        {
        }
#pragma warning restore CS8618
    }
}
