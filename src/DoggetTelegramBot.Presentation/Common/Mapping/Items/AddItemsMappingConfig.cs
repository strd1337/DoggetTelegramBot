using DoggetTelegramBot.Application.Items.Common;
using Mapster;

namespace DoggetTelegramBot.Presentation.Common.Mapping.Items
{
    public sealed class AddItemsMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config) =>
            config.NewConfig<AddItemsResult, Response>()
                .Map(dest => dest.Message, src => FormatAddItemsMessage());

        public string FormatAddItemsMessage() =>
            $"Successful adding! All items are saved.";
    }
}
