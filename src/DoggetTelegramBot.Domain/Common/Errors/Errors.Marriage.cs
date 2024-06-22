using ErrorOr;

namespace DoggetTelegramBot.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class Marriage
        {
            public static Error NotFound => Error.NotFound(
                code: "Marriage.NotFound",
                description: "Marriage is not found.");

            public static Error WrongType(string typeName) => Error.Conflict(
                code: "Marriage.DataConflict",
                description: $"{typeName} type needs to have less spouses.");

            public static Error TooManySpouses(int count) => Error.Conflict(
                code: "Marriage.DataConflict",
                description: $"You cannot have {count} spouses. " +
                    $"Max value is {Constants.Constants.Marriage.MaxSpousesCount}.");
        }
    }
}
