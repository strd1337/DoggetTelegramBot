using DoggetTelegramBot.Domain.Models.FamilyEntity;
using DoggetTelegramBot.Domain.Models.MarriageEntity;

namespace DoggetTelegramBot.Application.Marriages.Common
{
    public record DeleteMarriageResult(
        MarriageId MarriageId,
        FamilyId FamilyId);
}
