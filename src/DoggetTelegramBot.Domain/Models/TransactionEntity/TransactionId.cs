using DoggetTelegramBot.Domain.Common.Entities;

namespace DoggetTelegramBot.Domain.Models.TransactionEntity
{
    public sealed class TransactionId : RootId<Guid>
    {
        public override Guid Value { get; protected set; }

        private TransactionId(Guid value) => Value = value;

        public static TransactionId CreateUnique() => new(Guid.NewGuid());

        public static TransactionId Create(Guid value) => new(value);

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
