using DoggetTelegramBot.Domain.Models.MarriageEntity.Enums;

namespace DoggetTelegramBot.Application.DTOs
{
    public record MarriageDto(
        List<SpouseDto> Spouses,
        DateTime MarriageDate,
        DateTime? DivorceDate,
        MarriageType Type,
        MarriageStatus Status);
}
