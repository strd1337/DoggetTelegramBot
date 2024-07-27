using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Items.Common;
using DoggetTelegramBot.Domain.Models.ItemEntity.Enums;

namespace DoggetTelegramBot.Application.Items.Commands.Purchase
{
    public record PurchaseItemCommand(
        long TelegramId,
        ItemType Type,
        string ServerName,
        ItemAmountType? AmountType,
        int Count) : ICommand<PurchaseItemResult>;
}
