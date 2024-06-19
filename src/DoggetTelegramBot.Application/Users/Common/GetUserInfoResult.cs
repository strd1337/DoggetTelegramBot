using DoggetTelegramBot.Application.Marriages.Common;
using DoggetTelegramBot.Domain.Models.UserEntity.Enums;

namespace DoggetTelegramBot.Application.Users.Common
{
    public record GetUserInfoResult(
        string? Username,
        DateTime RegisteredDate,
        MaritalStatus MaritalStatus,
        List<UserPrivilege> Privileges,
        MarriageDto? Marriage);
}
