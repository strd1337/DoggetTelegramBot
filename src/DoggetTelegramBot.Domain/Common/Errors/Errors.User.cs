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
        }
    }
}
