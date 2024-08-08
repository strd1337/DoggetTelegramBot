using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Transactions.Common;

namespace DoggetTelegramBot.Application.Transactions.Commands
{
    public record RewardTransactionCommand(
        List<long> UserTelegramIds,
        decimal Amount) : ICommand<RewardTransactionResult>;
}
