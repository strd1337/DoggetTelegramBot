using DoggetTelegramBot.Domain.Models.MarriageEntity.Enums;
using DoggetTelegramBot.Presentation.Handlers.Common.Enums;
using PRTelegramBot.Interfaces;

namespace DoggetTelegramBot.Presentation.Handlers.Common.Caches
{
    public sealed class MarryStepCache : ITelegramCache
    {
        public MarriageType MarriageType { get; set; } = default!;
        public MarriageConfirmationCommands ConfirmationCommand { get; set; } = default!;

        public bool ClearData()
        {
            MarriageType = default;
            ConfirmationCommand = default;
            return true;
        }
    }
}
