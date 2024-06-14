using System.ComponentModel;

namespace DoggetTelegramBot.Domain.Models.MarriageEntity.Enums
{
    public enum MarriageStatus
    {
        [Description(nameof(Active))]
        Active,
        [Description(nameof(Divorced))]
        Divorced,
    }
}
