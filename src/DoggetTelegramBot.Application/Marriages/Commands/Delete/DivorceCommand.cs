using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Marriages.Common;

namespace DoggetTelegramBot.Application.Marriages.Commands.Delete
{
    public record DivorceCommand(List<long> Spouses) :
        ICommand<MarriageResult>;
}
