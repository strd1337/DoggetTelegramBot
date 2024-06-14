using DoggetTelegramBot.Domain.Common.Entities;

namespace DoggetTelegramBot.Domain.Models.MarriageEntity
{
    public sealed class MarriageId : RootId<Guid>
    {
        public override Guid Value { get; protected set; }

        private MarriageId(Guid value) => Value = value;

        public static MarriageId CreateUnique() => new(Guid.NewGuid());

        public static MarriageId Create(Guid value) => new(value);

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
