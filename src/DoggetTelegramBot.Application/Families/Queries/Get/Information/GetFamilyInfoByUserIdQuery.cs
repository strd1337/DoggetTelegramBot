using DoggetTelegramBot.Application.Common.Caching;
using DoggetTelegramBot.Application.Families.Common;
using DoggetTelegramBot.Application.Helpers;
using DoggetTelegramBot.Domain.Models.UserEntity;

namespace DoggetTelegramBot.Application.Families.Queries.Get.Information
{
    public record GetFamilyInfoByUserIdQuery(UserId UserId) :
        ICachedQuery<GetFamilyInfoResult>
    {
        public string CachedKey => CacheKeyGenerator.GetFamilyInfoByUserId(UserId);

        public TimeSpan? Expiration => null;
    }
}
