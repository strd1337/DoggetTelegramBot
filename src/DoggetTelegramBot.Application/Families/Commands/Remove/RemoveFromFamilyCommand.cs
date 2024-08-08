using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Families.Common;

namespace DoggetTelegramBot.Application.Families.Commands.Remove
{
    public record RemoveFromFamilyCommand(
        long ParentTelegramId,
        long MemberToRemoveTelegramId) : ICommand<RemoveFromFamilyResult>;
}
