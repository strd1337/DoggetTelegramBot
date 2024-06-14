using System.ComponentModel;

namespace DoggetTelegramBot.Domain.Models.UserEntity.Enums
{
    public enum MaritalStatus
    {
        [Description("Not married")]
        NotMarried,
        [Description(nameof(Married))]
        Married,
        [Description(nameof(Divorced))]
        Divorced,
    }
}
