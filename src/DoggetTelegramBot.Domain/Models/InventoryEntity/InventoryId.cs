using DoggetTelegramBot.Domain.Common.Entities;

namespace DoggetTelegramBot.Domain.Models.InventoryEntity
{
    public sealed class InventoryId : RootId<Guid>
    {
        public override Guid Value { get; protected set; }

        private InventoryId(Guid value) => Value = value;

        public static InventoryId CreateUnique() => new(Guid.NewGuid());

        public static InventoryId Create(Guid value) => new(value);

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
