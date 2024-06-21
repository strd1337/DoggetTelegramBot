using System.Text;
using DoggetTelegramBot.Application.Users.Common;
using Mapster;

namespace DoggetTelegramBot.Presentation.Common.Mapping.Users
{
    public class UpdateUserMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config) =>
            config.NewConfig<UpdateNicknameResult, Response>()
                .Map(dest => dest.Message, src => FormatUpdateUserMessage(
                    src.Nickname));

        private static string FormatUpdateUserMessage(
            string? nickname)
        {
            StringBuilder sb = new();

            sb.AppendLine(nickname is null ?
                $"You successfully removed your nickname." :
                $"Your nickname is {nickname ?? "none"}");

            return sb.ToString();
        }
    }
}
