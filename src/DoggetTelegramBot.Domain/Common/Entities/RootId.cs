namespace DoggetTelegramBot.Domain.Common.Entities
{
    public abstract class RootId<TId> : ValueObject
    {
        public abstract TId Value { get; protected set; }
    }
}
