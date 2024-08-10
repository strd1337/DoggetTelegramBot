using DoggetTelegramBot.Domain.Models.InventoryEntity;

namespace DoggetTelegramBot.Application.Helpers.CacheKeys
{
    public static class InventoryCacheKeyGenerator
    {
        private const string ModelName = nameof(Inventory);

        public static string GetInventoryInfoByTelegramId(long telegramId) =>
            CacheKeyGenerator.GenerateKey(ModelName, nameof(GetInventoryInfoByTelegramId), telegramId);

        public static string GetAllItemServerNames() =>
            CacheKeyGenerator.GenerateKey(ModelName, nameof(GetAllItemServerNames));
    }

}
