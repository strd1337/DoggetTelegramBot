using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Families.Common;
using DoggetTelegramBot.Domain.Models.UserEntity;

namespace DoggetTelegramBot.Application.Families.Commands.DeleteMember
{
    public record DeleteMemberFromAllFamiliesCommand(
        UserId UserId) : ICommand<DeleteMemberFromAllFamiliesResult>;
}
