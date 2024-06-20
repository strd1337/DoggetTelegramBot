using System.Text;
using DoggetTelegramBot.Application.Users.Common;
using Mapster;

namespace DoggetTelegramBot.Presentation.Common.Mapping.Users
{
    public class UpdateUserMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config) =>
            config.NewConfig<UpdateUsernameResult, Response>()
                .Map(dest => dest.Message, src => FormatMessage(
                    src.Username));

        private static string FormatMessage(
            string? username)
        {
            StringBuilder sb = new();
            sb.AppendLine($"Your username is {username ?? "none"}");

            return sb.ToString();
        }
    }
}
