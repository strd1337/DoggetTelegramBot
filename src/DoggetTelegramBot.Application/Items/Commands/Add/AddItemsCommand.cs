using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Items.Common;
using DoggetTelegramBot.Domain.Models.ItemEntity.Enums;

namespace DoggetTelegramBot.Application.Items.Commands.Add
{
    public record AddItemsCommand(
        ItemType Type,
        string ServerName,
        Dictionary<ItemAmountType, List<string>> Values) : ICommand<AddItemsResult>;
}
