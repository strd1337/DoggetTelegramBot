using System.Text;
using DoggetTelegramBot.Application.Users.Common;
using DoggetTelegramBot.Domain.Models.UserEntity.Enums;
using Mapster;
using PRTelegramBot.Extensions;

namespace DoggetTelegramBot.Presentation.Common.Mapping.Users
{
    public class GetUserInfoMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config) =>
            config.NewConfig<GetUserInfoResult, Response>()
                .Map(dest => dest.Message, src => FormatUserInfoMessage(
                    src.Username,
                    src.RegisteredDate,
                    src.MaritalStatus,
                    src.Privileges));

        private static string FormatUserInfoMessage(
            string? username,
            DateTime registeredDate,
            MaritalStatus maritalStatus,
            List<UserPrivilege> privileges)
        {
            StringBuilder sb = new();
            sb.AppendLine($"Username: {username ?? "none"}");
            sb.AppendLine($"Registered date: {registeredDate:dd-MM-yyyy}");
            sb.AppendLine($"Marital status: {maritalStatus.GetDescription()}");
            sb.AppendLine($"Privileges: {(privileges.Count != 0 ? string.Join(", ", privileges) : "none")}");

            return sb.ToString();
        }
    }
}
