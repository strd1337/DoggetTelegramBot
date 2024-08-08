using DoggetTelegramBot.Domain.Models.FamilyEntity;

namespace DoggetTelegramBot.Application.Families.Common
{
    public record DeleteMemberFromAllFamiliesResult(List<FamilyId> FamilyIds);
}
