using System.Text;
using DoggetTelegramBot.Application.DTOs;
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
                    src.Nickname,
                    src.FirstName,
                    src.RegisteredDate,
                    src.MaritalStatus,
                    src.Privileges,
                    src.Marriages,
                    src.Families));

        private static string FormatUserInfoMessage(
            string? username,
            string? nickname,
            string firstName,
            DateTime registeredDate,
            MaritalStatus maritalStatus,
            List<UserPrivilege> privileges,
            List<MarriageDto> marriages,
            List<FamilyDto> families)
        {
            StringBuilder sb = new("User information:\n\n");

            sb.AppendLine(GetDisplayName(firstName, username, nickname));

            sb.AppendLine($"Registered date: {registeredDate:dd-MM-yyyy}");
            sb.AppendLine($"Marital status: {maritalStatus.GetDescription()}");
            sb.AppendLine($"Privileges: {(privileges.Count != 0 ? string.Join(", ", privileges) : "member")}");

            if (marriages.Count != 0)
            {
                FormatMarriagesInfo(ref sb, marriages);
            }

            if (families.Count != 0)
            {
                FormatFamiliesInfo(ref sb, families);
            }

            return sb.ToString();
        }

        private static void FormatMarriagesInfo(
            ref StringBuilder sb,
            List<MarriageDto> marriages)
        {
            sb.AppendLine("\n\nMarriages information:\n");

            foreach (var marriage in marriages)
            {
                sb.AppendLine(" Marriage:");

                string spouses = string.Join(", ", marriage.Spouses.Select(GetDisplayName));
                sb.AppendLine($"  Spouses: {spouses}");

                sb.AppendLine($"  Marriage date: {marriage.MarriageDate:dd-MM-yyyy}");
                if (marriage.DivorceDate is not null)
                {
                    sb.AppendLine($"  Divorce date: {marriage.DivorceDate:dd-MM-yyyy}");
                }

                sb.AppendLine($"  Type: {marriage.Type.GetDescription()}");
                sb.AppendLine($"  Status: {marriage.Status.GetDescription()}\n");
            }
        }

        private static void FormatFamiliesInfo(
            ref StringBuilder sb,
            List<FamilyDto> families)
        {
            sb.AppendLine("\nFamilies information:");

            foreach (var family in families)
            {
                sb.AppendLine($"\nFamily:");
                foreach (var member in family.Members)
                {
                    sb.AppendLine($" Members:");
                    sb.AppendLine($" {GetDisplayName(member.FirstName, member.Username, member.Nickname)}");

                    sb.AppendLine($" Role: {member.Role.GetDescription()}");
                }
            }
        }

        private static string GetDisplayName(SpouseDto spouse) =>
            spouse.Nickname ?? spouse.Username ?? spouse.FirstName;

        private static string GetDisplayName(string firstName, string? username, string? nickname) =>
            nickname is not null ?
            $"Nickname: {nickname}" :
            username is not null ?
            $"Username: {username}" :
            $"First name: {firstName}";
    }
}
