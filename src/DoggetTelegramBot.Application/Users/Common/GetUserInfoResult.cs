using DoggetTelegramBot.Domain.Models.UserEntity.Enums;

namespace DoggetTelegramBot.Application.Users.Common
{
    public record GetUserInfoResult(
        string? Nickname,
        string? FirstName,
        string? LastName,
        DateTime RegisteredDate,
        MaritalStatus MaritalStatus,
        List<UserPrivilege> Privileges);
}
