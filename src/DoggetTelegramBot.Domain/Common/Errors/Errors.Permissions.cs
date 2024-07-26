using ErrorOr;

namespace DoggetTelegramBot.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class Permissions
        {
            public static Error UsageTime(TimeSpan duration, DateTime allowedAccessDate) => Error.Forbidden(
                code: "Permission.UsageTime",
                description: $"You can only use this command once per {duration.Days} day. You are allowed to use after {allowedAccessDate}.");
        }
    }
}
