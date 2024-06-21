using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Users.Common;

namespace DoggetTelegramBot.Application.Users.Commands.Update.Nickname
{
    public record UpdateNicknameByTelegramIdCommand(
        long TelegramId,
        string? Nickname) : ICommand<UpdateNicknameResult>;
}
