using DoggetTelegramBot.Domain.Models.UserEntity;

namespace DoggetTelegramBot.Application.Helpers
{
    public static class CacheKeyGenerator
    {
        public static string UserExistsWithTelegramId(long telegramId) =>
            $"user-exists-with-telegramId-{telegramId}";

        public static string GetUserInfoByTelegramId(long telegramId) =>
            $"get-user-info-by-telegramId-{telegramId}";

        public static string GetUserByTelegramId(long telegramId) =>
            $"get-user-by-telegramId-{telegramId}";

        public static string GetUserPrivilegesByTelegramId(long telegramId) =>
            $"get-user-privileges-by-telegramId-{telegramId}";

        public static string GetSpousesByTelegramIdsQuery(List<long> telegramIds) =>
           $"get-spouses-by-telegramIds-{string.Join(",", telegramIds.Select(id => id))}";

        public static string GetTransactionParticipantsByTelegramIdsQuery(long? fromTelegramId, long toTelegramId) =>
           $"get-transaction-participants-by-telegramIds-{(fromTelegramId is null ? toTelegramId : $"{fromTelegramId},{toTelegramId}")}";


        public static string GetAllMarriagesInfoByUserId(UserId userId) =>
            $"get-all-marriages-info-by-userId-{userId.Value}";


        public static string GetFamilyInfoByUserId(UserId userId) =>
            $"get-family-info-by-userId-{userId.Value}";


        public static string GetInventoryInfoByTelegramId(long telegramId) =>
            $"get-inventory-info-by-telegramId-{telegramId}";
    }
}
