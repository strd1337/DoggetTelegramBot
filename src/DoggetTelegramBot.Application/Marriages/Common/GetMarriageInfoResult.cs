using DoggetTelegramBot.Domain.Models.MarriageEntity.Enums;
using DoggetTelegramBot.Domain.Models.UserEntity;

namespace DoggetTelegramBot.Application.Marriages.Common
{
    public record GetMarriageInfoResult(
        List<UserId> SpouseIds,
        DateTime MarriageDate,
        DateTime? DivorceDate,
        MarriageType Type,
        MarriageStatus Status);
}
