namespace DoggetTelegramBot.Domain.Common.Entities
{
    public abstract class Root<TId, TIdType> : Entity<TId>
        where TId : RootId<TIdType>
    {
        public new RootId<TIdType> Id { get; protected set; }

        protected Root(TId id) => Id = id;

#pragma warning disable CS8618
        protected Root()
        {
        }
#pragma warning restore CS8618
    }
}
