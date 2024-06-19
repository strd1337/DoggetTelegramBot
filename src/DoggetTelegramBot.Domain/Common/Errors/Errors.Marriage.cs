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
        }
    }
}
