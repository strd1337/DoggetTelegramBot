using DoggetTelegramBot.Domain.Common.Entities;
using DoggetTelegramBot.Domain.Models.TransactionEntity.Enums;
using DoggetTelegramBot.Domain.Models.UserEntity;

namespace DoggetTelegramBot.Domain.Models.TransactionEntity
{
    public sealed class Transaction : Entity
    {
        public TransactionId TransactionId { get; private set; }
        public List<UserId> FromUserIds { get; private set; } = [];
        public UserId? ToUserId { get; private set; }
        public decimal Amount { get; private set; }
        public TransactionType Type { get; private set; }

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
            decimal amount) => new()
            {
                TransactionId = TransactionId.CreateUnique(),
                FromUserIds = [fromUserId],
                ToUserId = toUserId,
                Amount = amount,
                Type = TransactionType.Transfer
            };

        public static Transaction CreatePurchase(
            UserId toUserId,
            decimal amount) => new()
            {
                TransactionId = TransactionId.CreateUnique(),
                ToUserId = toUserId,
                Amount = amount,
                Type = TransactionType.Purchase
            };

        public static Transaction CreateReward(
            UserId fromUserId,
            UserId toUserId,
            decimal amount) => new()
            {
                TransactionId = TransactionId.CreateUnique(),
                FromUserIds = [fromUserId],
                ToUserId = toUserId,
                Amount = amount,
                Type = TransactionType.Reward
            };

        public static Transaction CreatePenalty(
            UserId fromUserId,
            UserId toUserId,
            decimal amount) => new()
            {
                TransactionId = TransactionId.CreateUnique(),
                FromUserIds = [fromUserId],
                ToUserId = toUserId,
                Amount = amount,
                Type = TransactionType.Penalty
            };

#pragma warning disable CS8618
        private Transaction()
        {
        }
#pragma warning restore CS8618
    }
}
