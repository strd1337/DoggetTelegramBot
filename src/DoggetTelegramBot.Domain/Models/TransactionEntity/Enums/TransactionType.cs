using System.ComponentModel;

namespace DoggetTelegramBot.Domain.Models.TransactionEntity.Enums
{
    public enum TransactionType
    {
        [Description(nameof(Transfer))]
        Transfer,
        [Description(nameof(Purchase))]
        Purchase,
        [Description(nameof(Reward))]
        Reward,
        [Description(nameof(Penalty))]
        Penalty,
        [Description("Service fee")]
        ServiceFee,
    }
}
