using System.Text;
using DoggetTelegramBot.Application.DTOs;
using DoggetTelegramBot.Application.Marriages.Common;
using Mapster;

namespace DoggetTelegramBot.Presentation.Common.Mapping.Marriages
{
    public class MarriageMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config) =>
            config.NewConfig<MarriageResult, Response>()
                .Map(dest => dest.Message, src => FormatMarriageMessage(
                    src.Spouses,
                    src.IsGetMarried));

        private static string FormatMarriageMessage(
            List<SpouseDto> spouses,
            bool isGetMarried = true)
        {
            if (spouses.Count != 2)
            {
                throw new ArgumentException("The list must contain exactly two spouses.");
            }

            StringBuilder sb = new();

            var requester = spouses[0];
            var recipient = spouses[1];

            if (isGetMarried)
            {
                sb.Append("Congratulations! ");
            }
            else
            {
                sb.Append("We regret to inform that ");
            }

            sb.Append(requester.Nickname is not null ?
                $"{requester.Nickname}" :
                requester.Username is not null ?
                $"@{requester.Username}" :
                $"{requester.FirstName}");

            sb.Append(" and ");

            sb.Append(recipient.Nickname is not null ?
                $"{recipient.Nickname}" :
                recipient.Username is not null ?
                $"@{recipient.Username}" :
                $"{recipient.FirstName}");

            if (isGetMarried)
            {
                sb.Append(" got married.");
            }
            else
            {
                sb.Append(" have decided to part ways.");
            }

            return sb.ToString();
        }
    }
}
