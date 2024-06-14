using DoggetTelegramBot.Domain.Common.Entities;

namespace DoggetTelegramBot.Domain.Models.FamilyEntity
{
    public sealed class FamilyId : RootId<Guid>
    {
        public override Guid Value { get; protected set; }

        private FamilyId(Guid value) => Value = value;

        public static FamilyId CreateUnique() => new(Guid.NewGuid());

        public static FamilyId Create(Guid value) => new(value);

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
