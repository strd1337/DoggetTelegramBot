using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Families.Common;
using DoggetTelegramBot.Domain.Models.UserEntity;

namespace DoggetTelegramBot.Application.Families.Commands.Delete
{
    public record DeleteFamilyCommand(
        List<UserId> SpouseIds) : ICommand<FamilyResult>;
}
