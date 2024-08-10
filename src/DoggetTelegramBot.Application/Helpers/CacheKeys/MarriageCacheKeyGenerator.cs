using DoggetTelegramBot.Domain.Models.MarriageEntity;
using DoggetTelegramBot.Domain.Models.UserEntity;

namespace DoggetTelegramBot.Application.Helpers.CacheKeys
{
    public static class MarriageCacheKeyGenerator
    {
        private const string ModelName = nameof(Marriage);

        public static string GetAllMarriagesInfoByUserId(UserId userId) =>
            CacheKeyGenerator.GenerateKey(ModelName, nameof(GetAllMarriagesInfoByUserId), userId.Value.ToString());
    }
}
