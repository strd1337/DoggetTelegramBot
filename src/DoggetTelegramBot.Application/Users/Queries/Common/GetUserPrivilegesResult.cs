using DoggetTelegramBot.Domain.Models.UserEntity.Enums;

namespace DoggetTelegramBot.Application.Users.Queries.Common
{
    public record GetUserPrivilegesResult(List<UserPrivilege> UserPrivileges);
}
