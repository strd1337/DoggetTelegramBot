using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Users.Common;

namespace DoggetTelegramBot.Application.Users.Commands.Delete
{
    public record DeleteUserByTelegramIdCommand(
        long TelegramId,
        bool IsDeleted = true) : ICommand<DeleteUserResult>;
}
