using DoggetTelegramBot.Domain.Models.FamilyEntity;
using DoggetTelegramBot.Domain.Models.UserEntity;

namespace DoggetTelegramBot.Application.Helpers.CacheKeys
{
    public static class FamilyCacheKeyGenerator
    {
        private const string ModelName = nameof(Family);

        public static string GetFamilyInfoByUserId(UserId userId) =>
            CacheKeyGenerator.GenerateKey(ModelName, nameof(GetFamilyInfoByUserId), userId.Value.ToString());
    }
}
