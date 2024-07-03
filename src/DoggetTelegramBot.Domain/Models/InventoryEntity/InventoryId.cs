namespace DoggetTelegramBot.Domain.Models.InventoryEntity
{
    public record InventoryId
    {
        public Guid Value { get; protected set; }

        private InventoryId(Guid value) => Value = value;

        public static InventoryId CreateUnique() => new(Guid.NewGuid());

        public static InventoryId Create(Guid value) => new(value);
    }
}
