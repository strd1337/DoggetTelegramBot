namespace DoggetTelegramBot.Domain.Models.FamilyEntity
{
    public record FamilyId
    {
        public Guid Value { get; protected set; }

        private FamilyId(Guid value) => Value = value;

        public static FamilyId CreateUnique() => new(Guid.NewGuid());

        public static FamilyId Create(Guid value) => new(value);
    }
}
