using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Inventories.Common;
using DoggetTelegramBot.Domain.Models.InventoryEntity;

namespace DoggetTelegramBot.Application.Inventories.Commands.Delete
{
    public record DeleteInventoryByInventoryIdCommand(
        InventoryId InventoryId,
        decimal Amount = 0,
        bool IsDeleted = true) : ICommand<DeleteInventoryResult>;
}
