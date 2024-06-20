using DoggetTelegramBot.Domain.Models.FamilyEntity;

namespace DoggetTelegramBot.Application.Families.Common
{
    public record GetFamilyInfoResult(
        List<FamilyMember> Members);
}
