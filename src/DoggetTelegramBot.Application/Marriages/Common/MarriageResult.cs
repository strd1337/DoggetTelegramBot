using DoggetTelegramBot.Application.DTOs;

namespace DoggetTelegramBot.Application.Marriages.Common
{
    public record MarriageResult(
        List<SpouseDto> Spouses,
        bool IsGetMarried = true);
}
