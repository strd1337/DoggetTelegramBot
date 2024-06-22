using DoggetTelegramBot.Application.Common.CQRS;
using PRTelegramBot.Models.Enums;

namespace DoggetTelegramBot.Application.Users.Commands.Register
{
    public record RegisterUserCommand(
        long TelegramId,
        string? Username) : ICommand<UpdateResult>;
}
