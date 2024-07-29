using DoggetTelegramBot.Application.Families.Common;
using Mapster;

namespace DoggetTelegramBot.Presentation.Common.Mapping.Families
{
    public sealed class AddToFamilyMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config) =>
            config.NewConfig<FamilyResult, Response>()
                .Map(dest => dest.Message, src => FormatAddToFamilyMessage());

        public string FormatAddToFamilyMessage() =>
            $"The user is successfully added to your family.";
    }
}
