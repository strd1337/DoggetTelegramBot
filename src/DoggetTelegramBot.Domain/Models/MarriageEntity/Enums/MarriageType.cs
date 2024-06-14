using System.ComponentModel;

namespace DoggetTelegramBot.Domain.Models.MarriageEntity.Enums
{
    public enum MarriageType
    {
        [Description(nameof(Civil))]
        Civil,
        [Description(nameof(Religious))]
        Religious,
        [Description("Same-Sex")]
        SameSex,
        [Description(nameof(Polygamous))]
        Polygamous,
        [Description("Common-Law")]
        CommonLaw,
    }
}
