using DoggetTelegramBot.Application.Common.Caching;
using DoggetTelegramBot.Application.Helpers;
using DoggetTelegramBot.Application.Inventories.Common;

namespace DoggetTelegramBot.Application.Inventories.Queries.Get.Info
{
    public record GetInventoryInfoByTelegramIdQuery(
        long TelegramId) : ICachedQuery<GetInventoryResult>
    {
        public string CachedKey => CacheKeyGenerator.GetInventoryInfoByTelegramId(TelegramId);

        public TimeSpan? Expiration => null;
    }
}
