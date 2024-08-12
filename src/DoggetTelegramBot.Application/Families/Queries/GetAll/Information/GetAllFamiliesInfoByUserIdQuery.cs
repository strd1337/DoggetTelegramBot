using DoggetTelegramBot.Application.Common.Caching;
using DoggetTelegramBot.Application.Families.Common;
using DoggetTelegramBot.Application.Helpers.CacheKeys;
using DoggetTelegramBot.Domain.Models.UserEntity;

namespace DoggetTelegramBot.Application.Families.Queries.GetAll.Information
{
    public record GetAllFamiliesInfoByUserIdQuery(UserId UserId) :
        ICachedQuery<GetAllFamiliesInfoResult>
    {
        public string CachedKey => FamilyCacheKeyGenerator.GetFamilyInfoByUserId(UserId);

        public TimeSpan? Expiration => null;
    }
}
