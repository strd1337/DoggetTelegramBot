using DoggetTelegramBot.Application.DTOs;
using DoggetTelegramBot.Domain.Models.UserEntity.Enums;

namespace DoggetTelegramBot.Application.Users.Common
{
    public record GetUserInfoResult(
        string? Username,
        string? Nickname,
        DateTime RegisteredDate,
        MaritalStatus MaritalStatus,
        List<UserPrivilege> Privileges,
        MarriageDto? Marriage,
        FamilyDto? Family);
}
