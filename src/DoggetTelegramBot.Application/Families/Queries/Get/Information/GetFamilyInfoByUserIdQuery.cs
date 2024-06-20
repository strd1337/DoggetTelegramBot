using DoggetTelegramBot.Application.Common.Caching;
using DoggetTelegramBot.Application.Families.Common;
using DoggetTelegramBot.Domain.Models.UserEntity;

namespace DoggetTelegramBot.Application.Families.Queries.Get.Information
{
    public record GetFamilyInfoByUserIdQuery(UserId UserId) :
        ICachedQuery<GetFamilyInfoResult>
    {
        public string CachedKey => $"get-family-info-by-userId-{UserId.Value}";

        public TimeSpan? Expiration => null;
    }
}
