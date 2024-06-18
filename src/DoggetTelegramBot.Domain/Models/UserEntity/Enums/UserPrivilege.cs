using System.ComponentModel;

namespace DoggetTelegramBot.Domain.Models.UserEntity.Enums
{
    [Flags]
    public enum UserPrivilege
    {
        [Description("VIP")]
        VIP = 1,
        [Description("Moderator")]
        Moderator = 2,
        [Description("Admin")]
        Admin = 4,
    }
}
