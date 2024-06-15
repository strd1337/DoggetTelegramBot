using DoggetTelegramBot.Application.Common.CQRS;

namespace DoggetTelegramBot.Application.Common.Caching
{
    public interface ICachedQuery
    {
        string CachedKey { get; }
        TimeSpan? Expiration { get; }
    }

    public interface ICachedQuery<TResponse> : IQuery<TResponse>, ICachedQuery
    {
    }
}
