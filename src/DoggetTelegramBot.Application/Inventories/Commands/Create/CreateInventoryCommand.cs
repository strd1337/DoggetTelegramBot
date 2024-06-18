using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Inventories.Common;

namespace DoggetTelegramBot.Application.Inventories.Commands.Create
{
    public record CreateInventoryCommand() : ICommand<CreateInventoryResult>;
}
