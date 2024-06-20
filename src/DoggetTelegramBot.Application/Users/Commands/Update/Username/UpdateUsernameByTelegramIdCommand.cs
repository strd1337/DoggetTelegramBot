using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Users.Common;

namespace DoggetTelegramBot.Application.Users.Commands.Update.Username
{
    public record UpdateUsernameByTelegramIdCommand(
        long TelegramId,
        string? Username) : ICommand<UpdateUsernameResult>;
}
