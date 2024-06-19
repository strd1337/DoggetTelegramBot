using System.Text;
using DoggetTelegramBot.Application.Marriages.Common;
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
                    src.Privileges,
                    src.Marriage));

        private static string FormatUserInfoMessage(
            string? username,
            DateTime registeredDate,
            MaritalStatus maritalStatus,
            List<UserPrivilege> privileges,
            MarriageDto? marriage)
        {
            StringBuilder sb = new("User information:\n");
            sb.AppendLine($"Username: {username ?? "none"}");
            sb.AppendLine($"Registered date: {registeredDate:dd-MM-yyyy}");
            sb.AppendLine($"Marital status: {maritalStatus.GetDescription()}");
            sb.AppendLine($"Privileges: {(privileges.Count != 0 ? string.Join(", ", privileges) : "member")}");

            if (marriage is not null)
            {
                FormatMarriageInfo(ref sb, marriage);
            }

            return sb.ToString();
        }

        private static void FormatMarriageInfo(
            ref StringBuilder sb,
            MarriageDto marriage)
        {
            sb.AppendLine("\nMarriage information:");
            sb.Append($"Spouse{(marriage.Spouses.Count > 1 ? "s" : string.Empty)}:");
            foreach (var spouse in marriage.Spouses)
            {
                sb.Append($" {spouse.Username}");
            }

            sb.AppendLine($"\nMarriage date: {marriage.MarriageDate:dd-MM-yyyy}");
            if (marriage.DivorceDate is not null)
            {
                sb.AppendLine($"Divorce date: {marriage.DivorceDate:dd-MM-yyyy}");
            }

            sb.AppendLine($"Type: {marriage.Type.GetDescription()}");
            sb.AppendLine($"Status: {marriage.Status.GetDescription()}");
        }
    }
}
