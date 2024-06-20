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
        }
    }
}
