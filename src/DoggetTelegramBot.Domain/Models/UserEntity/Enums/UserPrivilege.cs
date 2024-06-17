using System.ComponentModel;

namespace DoggetTelegramBot.Domain.Models.UserEntity.Enums
{
    [Flags]
    public enum UserPrivilege
    {
        [Description("Guest")]
        Guest = 1,
        [Description("Registered")]
        Registered = 2,
        [Description("Admin")]
        Admin = 4,
        [Description("VIP")]
        VIP = 8,
        [Description("Moderator")]
        Moderator = 16,
    }
}
