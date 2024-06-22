using DoggetTelegramBot.Application.Marriages.Common;
using Mapster;

namespace DoggetTelegramBot.Presentation.Common.Mapping.Marriages
{
    public class CreateMarriageMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config) =>
            config.NewConfig<CreateMarriageResult, Response>()
                .Map(dest => dest.Message, src => FormatMessage());

        private static string FormatMessage() =>
            $"Marriage was successfully created.";
    }
}
