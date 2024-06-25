using DoggetTelegramBot.Application.Common.Caching;
using DoggetTelegramBot.Application.Helpers;
using DoggetTelegramBot.Application.Marriages.Common;
using DoggetTelegramBot.Domain.Models.UserEntity;

namespace DoggetTelegramBot.Application.Marriages.Queries.GetAll.Information
{
    public record GetAllMarriagesInfoByUserIdQuery(UserId UserId) :
        ICachedQuery<GetAllMarriagesInfoResult>
    {
        public string CachedKey => CacheKeyGenerator.GetAllMarriagesInfoByUserId(UserId);

        public TimeSpan? Expiration => null;
    }
}
