using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Marriages.Common;
using DoggetTelegramBot.Domain.Models.MarriageEntity.Enums;

namespace DoggetTelegramBot.Application.Marriages.Commands.Create
{
    public record CreateMarriageCommand(
        MarriageType Type,
        List<long> Spouses) : ICommand<CreateMarriageResult>;
}
