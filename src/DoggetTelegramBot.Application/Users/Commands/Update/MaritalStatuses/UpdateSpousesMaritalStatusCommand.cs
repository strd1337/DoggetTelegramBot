using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Users.Common;
using DoggetTelegramBot.Domain.Models.UserEntity;
using DoggetTelegramBot.Domain.Models.UserEntity.Enums;

namespace DoggetTelegramBot.Application.Users.Commands.Update.MaritalStatuses
{
    public record UpdateSpousesMaritalStatusCommand(
        List<User> Spouses,
        MaritalStatus MaritalStatus) : ICommand<UpdateSpousesMaritalStatusResult>;
}
