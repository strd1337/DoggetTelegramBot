using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Inventories.Common;
using DoggetTelegramBot.Domain.Models.InventoryEntity;

namespace DoggetTelegramBot.Application.Inventories.Commands.Update
{
    public record UpdateInventoryByInventoryIdCommand(
        InventoryId InventoryId,
        decimal NewAmount = 0,
        bool IsDeleted = false) : ICommand<UpdateInventoryResult>;
}
