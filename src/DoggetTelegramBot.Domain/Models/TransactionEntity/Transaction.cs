using DoggetTelegramBot.Domain.Common.Entities;
using DoggetTelegramBot.Domain.Models.ItemEntity;
using DoggetTelegramBot.Domain.Models.TransactionEntity.Enums;
using DoggetTelegramBot.Domain.Models.UserEntity;

namespace DoggetTelegramBot.Domain.Models.TransactionEntity
{
    public sealed class Transaction : Root<TransactionId, Guid>
    {
        private readonly List<ItemId> itemIds = [];

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
                FromUserIds = [fromUserId],
                ToUserIds = [toUserId],
                Amount = amount,
                Type = TransactionType.Penalty
            };
            transaction.itemIds.AddRange(items);
            return transaction;
        }

        private Transaction() { }
    }
}
