using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Marriages.Common;
using DoggetTelegramBot.Domain.Models.UserEntity;

namespace DoggetTelegramBot.Application.Marriages.Commands.Delete
{
    public record DeleteMarriageCommand(
        UserId UserId) : ICommand<DeleteMarriageResult>;
}
