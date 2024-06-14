using DoggetTelegramBot.Domain.Common.Entities;

namespace DoggetTelegramBot.Domain.Models.UserEntity
{
    public sealed class UserId : RootId<Guid>
    {
        public override Guid Value { get; protected set; }

        private UserId(Guid value) => Value = value;

        public static UserId CreateUnique() => new(Guid.NewGuid());

        public static UserId Create(Guid value) => new(value);

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
