using DoggetTelegramBot.Application.Items.Common;
using Mapster;

namespace DoggetTelegramBot.Presentation.Common.Mapping.Items
{
    public class PurchaseItemMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config) =>
            config.NewConfig<PurchaseItemResult, Response>()
                .Map(dest => dest.Message, src => FormatPurchaseItemMessage(src.Value));

        public string FormatPurchaseItemMessage(string value) =>
            $"Successful purchase, {value} is yours.";
    }
}
