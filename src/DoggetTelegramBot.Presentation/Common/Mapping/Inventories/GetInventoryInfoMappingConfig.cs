using System.Text;
using DoggetTelegramBot.Application.Inventories.Common;
using Mapster;

namespace DoggetTelegramBot.Presentation.Common.Mapping.Inventories
{
    public sealed class GetInventoryInfoMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config) =>
            config.NewConfig<GetInventoryResult, Response>()
                .Map(dest => dest.Message, src => FormatGetInvetoryMessage(
                    src.YuanBalance));

        private static string FormatGetInvetoryMessage(
           decimal yuanBalance)
        {
            StringBuilder sb = new();
            sb.AppendLine($"Your balance is {yuanBalance} yuan.");
            return sb.ToString();
        }
    }
}
