namespace DoggetTelegramBot.Domain.Models.UserEntity
{
    public record UserId
    {
        public Guid Value { get; protected set; }

        private UserId(Guid value) => Value = value;

        public static UserId CreateUnique() => new(Guid.NewGuid());

        public static UserId Create(Guid value) => new(value);
    }
}
