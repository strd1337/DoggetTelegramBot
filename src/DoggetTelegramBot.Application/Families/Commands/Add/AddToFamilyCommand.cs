using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Families.Common;
using DoggetTelegramBot.Domain.Models.FamilyEntity.Enums;

namespace DoggetTelegramBot.Application.Families.Commands.Add
{
    public record AddToFamilyCommand(
        long ParentTelegramId,
        long NewMemberTelegramId,
        FamilyRole FamilyRole) : ICommand<AddToFamilyResult>;
}
