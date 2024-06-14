using DoggetTelegramBot.Domain.Common.Entities;

namespace DoggetTelegramBot.Domain.Models.ItemEntity
{
    public sealed class ItemId : RootId<Guid>
    {
        public override Guid Value { get; protected set; }

        private ItemId(Guid value) => Value = value;

        public static ItemId CreateUnique() => new(Guid.NewGuid());

        public static ItemId Create(Guid value) => new(value);

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
