using ErrorOr;

namespace DoggetTelegramBot.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class User
        {
            public static Error NotFound => Error.NotFound(
                code: "User.NotFound",
                description: "User is not found.");

            public static Error SomeMarried => Error.NotFound(
                code: "Users.SomeMarried",
                description: "Some of the provided users were married.");

            public static Error SomeNotMarriedOrDivorced => Error.NotFound(
                code: "Users.SomeNotMarried",
                description: "Some of the users provided were unmarried or divorced.");
        }
    }
}
