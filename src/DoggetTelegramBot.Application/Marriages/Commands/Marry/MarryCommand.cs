using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Marriages.Common;
using DoggetTelegramBot.Domain.Models.MarriageEntity.Enums;

namespace DoggetTelegramBot.Application.Marriages.Commands.Marry
{
    public record MarryCommand(
        MarriageType Type,
        List<long> Spouses) : ICommand<MarriageResult>;
}
