using DoggetTelegramBot.Application.Common.Caching;
using DoggetTelegramBot.Application.Helpers;
using DoggetTelegramBot.Application.Items.Common;

namespace DoggetTelegramBot.Application.Items.Queries.Server
{
    public record GetAllItemServerNamesQuery() : ICachedQuery<ItemServerNamesResult>
    {
        public string CachedKey => CacheKeyGenerator.GetAllItemServerNames();

        public TimeSpan? Expiration => TimeSpan.FromDays(1);
    }
}
