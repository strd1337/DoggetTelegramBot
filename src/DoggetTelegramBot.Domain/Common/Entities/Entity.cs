namespace DoggetTelegramBot.Domain.Common.Entities
{
    public abstract class Entity<TId> : IEquatable<Entity<TId>>
        where TId : ValueObject
    {
        public TId Id { get; protected set; }
        public bool IsDeleted { get; protected set; }
        public DateTime CreatedDate { get; protected set; } = DateTime.UtcNow;
        public DateTime ModifiedDate { get; protected set; } = DateTime.UtcNow;

        protected Entity(TId id) => Id = id;

        public override bool Equals(object? obj)
            => obj is Entity<TId> entity && Id.Equals(entity.Id);

        public bool Equals(Entity<TId>? other) => Equals((object?)other);

        public static bool operator ==(Entity<TId> left, Entity<TId> right)
            => Equals(left, right);

        public static bool operator !=(Entity<TId> left, Entity<TId> right)
            => !Equals(left, right);

        public override int GetHashCode() => Id.GetHashCode();

#pragma warning disable CS8618
        protected Entity()
        {
        }
#pragma warning restore CS8618
    }
}
