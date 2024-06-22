using DoggetTelegramBot.Domain.Models.UserEntity;

namespace DoggetTelegramBot.Application.Helpers
{
    public static class CacheKeyGenerator
    {
        public static string UserExistsWithTelegramId(long telegramId) =>
            $"user-exists-with-telegramId-{telegramId}";

        public static string GetUserInfoByTelegramId(long telegramId) =>
            $"get-user-info-by-telegramId-{telegramId}";

        public static string GetUserPrivilegesByTelegramId(long telegramId) =>
            $"get-user-privileges-by-telegramId-{telegramId}";

        public static string GetUsersByTelegramIdsQuery(List<long> telegramIds) =>
           $"get-users-by-telegramIds-{string.Join(",", telegramIds.Select(id => id))}";

        public static string GetMarriageInfoByUserId(UserId userId) =>
            $"get-marriage-info-by-userId-{userId.Value}";

        public static string GetFamilyInfoByUserId(UserId userId) =>
            $"get-family-info-by-userId-{userId.Value}";
    }
}
