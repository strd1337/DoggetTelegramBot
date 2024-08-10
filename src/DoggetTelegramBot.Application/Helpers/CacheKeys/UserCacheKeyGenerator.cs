using DoggetTelegramBot.Domain.Models.UserEntity;

namespace DoggetTelegramBot.Application.Helpers.CacheKeys
{
    public static class UserCacheKeyGenerator
    {
        private const string ModelName = nameof(User);

        public static string UserExistsWithTelegramId(long telegramId) =>
            CacheKeyGenerator.GenerateKey(ModelName, nameof(UserExistsWithTelegramId), telegramId);

        public static string GetUserInfoByTelegramId(long telegramId) =>
            CacheKeyGenerator.GenerateKey(ModelName, nameof(GetUserInfoByTelegramId), telegramId);

        public static string GetUserByTelegramId(long telegramId) =>
            CacheKeyGenerator.GenerateKey(ModelName, nameof(GetUserByTelegramId), telegramId);

        public static string GetUserPrivilegesByTelegramId(long telegramId) =>
            CacheKeyGenerator.GenerateKey(ModelName, nameof(GetUserPrivilegesByTelegramId), telegramId);

        public static string UserMessageCountByTelegramId(long telegramId) =>
            CacheKeyGenerator.GenerateKey(ModelName, nameof(UserMessageCountByTelegramId), telegramId);

        public static string GetSpousesByTelegramIdsQuery(List<long> telegramIds) =>
            CacheKeyGenerator.GenerateKey(ModelName, nameof(GetSpousesByTelegramIdsQuery), telegramIds);

        public static string GetAllUsersByTelegramIds(List<long> telegramIds) =>
            CacheKeyGenerator.GenerateKey(ModelName, nameof(GetAllUsersByTelegramIds), telegramIds);
    }
}
