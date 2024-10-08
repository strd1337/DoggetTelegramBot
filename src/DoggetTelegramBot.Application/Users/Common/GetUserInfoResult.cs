using DoggetTelegramBot.Application.DTOs;
using DoggetTelegramBot.Domain.Models.UserEntity.Enums;

namespace DoggetTelegramBot.Application.Users.Common
{
    public record GetUserInfoResult(
        string? Username,
        string? Nickname,
        string FirstName,
        DateTime RegisteredDate,
        MaritalStatus MaritalStatus,
        List<UserPrivilege> Privileges,
        List<MarriageDto> Marriages,
        List<FamilyDto> Families);
}
