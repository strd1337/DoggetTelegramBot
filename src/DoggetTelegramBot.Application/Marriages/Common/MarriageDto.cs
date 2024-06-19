using DoggetTelegramBot.Application.Users.Common;
using DoggetTelegramBot.Domain.Models.MarriageEntity.Enums;

namespace DoggetTelegramBot.Application.Marriages.Common
{
    public record MarriageDto(
        List<SpouseDto> Spouses,
        DateTime MarriageDate,
        DateTime? DivorceDate,
        MarriageType Type,
        MarriageStatus Status);
}
