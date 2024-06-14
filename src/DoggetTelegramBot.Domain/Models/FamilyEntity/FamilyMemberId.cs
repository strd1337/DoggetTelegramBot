using DoggetTelegramBot.Domain.Common.Entities;

namespace DoggetTelegramBot.Domain.Models.FamilyEntity
{
    public sealed class FamilyMemberId : RootId<Guid>
    {
        public override Guid Value { get; protected set; }

        private FamilyMemberId(Guid value) => Value = value;

        public static FamilyMemberId CreateUnique() => new(Guid.NewGuid());

        public static FamilyMemberId Create(Guid value) => new(value);

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
