using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Inventories.Common;

namespace DoggetTelegramBot.Application.Inventories.Commands.Transfer
{
    public record TransferMoneyCommand(
        long FromTelegramId,
        long ToTelegramId,
        decimal Amount) : ICommand<TransferMoneyResult>;
}
