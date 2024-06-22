using DoggetTelegramBot.Application.Common.CQRS;
using PRTelegramBot.Models.Enums;

namespace DoggetTelegramBot.Application.Users.Commands.Update.Details
{
    public record UpdateUserDetailsByTelegramIdCommand(
        long TelegramId,
        string? Username,
        string FirstName) : ICommand<UpdateResult>;
}
