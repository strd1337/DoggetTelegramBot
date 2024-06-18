using DoggetTelegramBot.Domain.Models.UserEntity.Enums;

namespace DoggetTelegramBot.Application.Users.Common
{
    public record GetUserPrivilegesResult(List<UserPrivilege> UserPrivileges);
}
