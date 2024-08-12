using DoggetTelegramBot.Application.Common.Caching;
using DoggetTelegramBot.Application.Helpers.CacheKeys;
using DoggetTelegramBot.Application.Marriages.Common;
using DoggetTelegramBot.Domain.Models.UserEntity;

namespace DoggetTelegramBot.Application.Marriages.Queries.GetAll.Information
{
    public record GetAllMarriagesInfoByUserIdQuery(UserId UserId) :
        ICachedQuery<GetAllMarriagesInfoResult>
    {
        public string CachedKey => MarriageCacheKeyGenerator.GetAllMarriagesInfoByUserId(UserId);

        public TimeSpan? Expiration => null;
    }
}
