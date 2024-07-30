using DoggetTelegramBot.Domain.Models.FamilyEntity.Enums;
using PRTelegramBot.Interfaces;

namespace DoggetTelegramBot.Presentation.Handlers.Common.Caches
{
    public sealed class AddToFamilyStepCache : ITelegramCache
    {
        public FamilyRole FamilyRole { get; set; } = default!;

        public bool ClearData()
        {
            FamilyRole = default;
            return true;
        }
    }
}
