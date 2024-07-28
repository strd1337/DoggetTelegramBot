using ErrorOr;

namespace DoggetTelegramBot.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class Family
        {
            public static Error NotFound => Error.NotFound(
                code: "Family.NotFound",
                description: "Family is not found.");

            public static Error SomeoneIsPet => Error.Conflict(
                code: "Family.SomeoneIsPet",
                description: "Some of you are a pet in another family.");
        }
    }
}
