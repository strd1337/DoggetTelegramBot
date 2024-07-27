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
                    src.Family));

        private static string FormatUserInfoMessage(
            string? username,
            string? nickname,
            string firstName,
            DateTime registeredDate,
            MaritalStatus maritalStatus,
            List<UserPrivilege> privileges,
            List<MarriageDto> marriages,
            FamilyDto? family)
        {
            StringBuilder sb = new("User information:\n");

            sb.AppendLine(nickname is not null ?
                $"Nickname: {nickname}" :
                username is not null ?
                $"Username: {username}" :
                $"First name: {firstName}");

            sb.AppendLine($"Registered date: {registeredDate:dd-MM-yyyy}");
            sb.AppendLine($"Marital status: {maritalStatus.GetDescription()}");
            sb.AppendLine($"Privileges: {(privileges.Count != 0 ? string.Join(", ", privileges) : "member")}");

            if (marriages.Count != 0)
            {
                FormatMarriagesInfo(ref sb, marriages);
            }

            if (family is not null)
            {
                FormatFamilyInfo(ref sb, family);
            }

            return sb.ToString();
        }

        private static void FormatMarriagesInfo(
            ref StringBuilder sb,
            List<MarriageDto> marriages)
        {
            sb.AppendLine("\nMarriages information:");

            foreach (var marriage in marriages)
            {
                sb.AppendLine(" Marriage:");

                sb.Append("  Spouses: ");
                foreach (var spouse in marriage.Spouses)
                {
                    if (spouse.Nickname is not null)
                    {
                        sb.Append($"  {spouse.Nickname} ");
                    }
                    else if (spouse.Username is not null)
                    {
                        sb.Append($"  {spouse.Username} ");
                    }
                    else
                    {
                        sb.Append($"  {spouse.FirstName} ");
                    }
                }

                sb.AppendLine($"\n  Marriage date: {marriage.MarriageDate:dd-MM-yyyy}");
                if (marriage.DivorceDate is not null)
                {
                    sb.AppendLine($"  Divorce date: {marriage.DivorceDate:dd-MM-yyyy}");
                }

                sb.AppendLine($"  Type: {marriage.Type.GetDescription()}");
                sb.AppendLine($"  Status: {marriage.Status.GetDescription()}");
                sb.AppendLine();
            }
        }

        private static void FormatFamilyInfo(
            ref StringBuilder sb,
            FamilyDto family)
        {
            sb.AppendLine("\nFamily information:");
            sb.AppendLine($"Members:");
            foreach (var member in family.Members)
            {
                sb.AppendLine(member.Nickname is not null ?
                     $" Nickname: {member.Nickname}" :
                     member.Username is not null ?
                     $" Username: {member.Username}" :
                     $" First name: {member.FirstName}");

                sb.AppendLine($" Role: {member.Role.GetDescription()}");
            }
        }
    }
}
