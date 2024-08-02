using DoggetTelegramBot.Domain.Models.UserEntity.Enums;

namespace DoggetTelegramBot.Application.Helpers
{
    public static class PrivilegeHelper
    {
        public static bool HasRequiredPrivilege(
            IEnumerable<UserPrivilege> userPrivileges,
            UserPrivilege requiredPrivileges) =>
                userPrivileges.Any(p => requiredPrivileges.HasFlag(p));
    }
}
