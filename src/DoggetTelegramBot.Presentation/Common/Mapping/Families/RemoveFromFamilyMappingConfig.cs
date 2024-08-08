using DoggetTelegramBot.Application.Families.Common;
using Mapster;

namespace DoggetTelegramBot.Presentation.Common.Mapping.Families
{
    public sealed class RemoveFromFamilyMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config) =>
            config.NewConfig<RemoveFromFamilyResult, Response>()
                .Map(dest => dest.Message, src => FormatRemoveFromFamilyMessage());

        public string FormatRemoveFromFamilyMessage() =>
            $"The user is successfully removed from your family.";
    }
}
