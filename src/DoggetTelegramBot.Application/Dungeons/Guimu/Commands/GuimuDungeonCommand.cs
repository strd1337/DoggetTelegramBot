using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Dungeons.Guimu.Common;

namespace DoggetTelegramBot.Application.Dungeons.Guimu.Commands
{
    public record GuimuDungeonCommand(
        long ParticipantTelegramId) : ICommand<GuimuDungeonResult>;
}
