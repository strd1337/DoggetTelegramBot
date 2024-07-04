namespace DoggetTelegramBot.Domain.Models.FamilyEntity
{
    public record FamilyMemberId
    {
        public Guid Value { get; protected set; }

        private FamilyMemberId(Guid value) => Value = value;

        public static FamilyMemberId CreateUnique() => new(Guid.NewGuid());

        public static FamilyMemberId Create(Guid value) => new(value);
    }
}
