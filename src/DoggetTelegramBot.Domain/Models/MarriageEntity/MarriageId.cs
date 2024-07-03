namespace DoggetTelegramBot.Domain.Models.MarriageEntity
{
    public record MarriageId
    {
        public Guid Value { get; protected set; }

        private MarriageId(Guid value) => Value = value;

        public static MarriageId CreateUnique() => new(Guid.NewGuid());

        public static MarriageId Create(Guid value) => new(value);
    }
}
