namespace DoggetTelegramBot.Domain.Models.ItemEntity
{
    public record ItemId
    {
        public Guid Value { get; protected set; }

        private ItemId(Guid value) => Value = value;

        public static ItemId CreateUnique() => new(Guid.NewGuid());

        public static ItemId Create(Guid value) => new(value);
    }
}
