using DoggetTelegramBot.Domain.Models.FamilyEntity;

namespace DoggetTelegramBot.Application.Families.Common
{
    public record GetAllFamiliesInfoResult(
        List<Family> Families);
}
