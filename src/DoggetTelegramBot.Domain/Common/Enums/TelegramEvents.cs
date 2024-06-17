using System.ComponentModel;

namespace DoggetTelegramBot.Domain.Common.Enums
{
    public enum TelegramEvents
    {
        [Description(nameof(All))]
        All,
        [Description(nameof(Initialization))]
        Initialization,
        [Description(nameof(Register))]
        Register,
        [Description(nameof(Message))]
        Message,
        [Description(nameof(Server))]
        Server,
        [Description(nameof(BlockedBot))]
        BlockedBot,
        [Description(nameof(CommandExecute))]
        CommandExecute,
        [Description(nameof(GroupAction))]
        GroupAction,
    }
}
