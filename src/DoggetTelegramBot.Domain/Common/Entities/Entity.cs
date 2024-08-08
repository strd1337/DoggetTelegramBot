namespace DoggetTelegramBot.Domain.Common.Entities
{
    public abstract class Entity : IEquatable<Entity>
    {
        public bool IsDeleted { get; protected set; }
        public DateTime CreatedDate { get; protected set; } = DateTime.UtcNow;
        public DateTime ModifiedDate { get; protected set; } = DateTime.UtcNow;

        public override bool Equals(object? obj) => obj is Entity;

        public bool Equals(Entity? other) => Equals((object?)other);

        public static bool operator ==(Entity left, Entity right)
            => Equals(left, right);

        public static bool operator !=(Entity left, Entity right)
            => !Equals(left, right);

        public override int GetHashCode()
        {
            HashCode hash = new();
            hash.Add(GetType());
            hash.Add(IsDeleted);
            hash.Add(CreatedDate);
            hash.Add(ModifiedDate);
            return hash.ToHashCode();
        }

        public virtual void Delete() => IsDeleted = true;

        public virtual void Restore() => IsDeleted = false;

        protected Entity()
        {
        }
    }
}
