using DoggetTelegramBot.Application.Common.Caching;
using DoggetTelegramBot.Application.Helpers;
using DoggetTelegramBot.Application.Marriages.Common;
using DoggetTelegramBot.Domain.Models.UserEntity;

namespace DoggetTelegramBot.Application.Marriages.Queries.Get.Information
{
    public record GetMarriageInfoByUserIdQuery(UserId UserId) :
        ICachedQuery<GetMarriageInfoResult>
    {
        public string CachedKey => CacheKeyGenerator.GetMarriageInfoByUserId(UserId);

        public TimeSpan? Expiration => null;
    }
}
