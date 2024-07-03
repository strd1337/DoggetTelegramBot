namespace DoggetTelegramBot.Domain.Models.TransactionEntity
{
    public record TransactionId
    {
        public Guid Value { get; protected set; }

        private TransactionId(Guid value) => Value = value;

        public static TransactionId CreateUnique() => new(Guid.NewGuid());

        public static TransactionId Create(Guid value) => new(value);
    }
}
