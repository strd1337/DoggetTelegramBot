using DoggetTelegramBot.Domain.Models.MarriageEntity;

namespace DoggetTelegramBot.Application.Marriages.Common
{
    public record GetAllMarriagesInfoResult(
        List<Marriage> Marriages);
}
