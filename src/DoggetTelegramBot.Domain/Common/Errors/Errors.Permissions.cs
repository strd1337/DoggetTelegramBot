using ErrorOr;
using Formatters = DoggetTelegramBot.Domain.Common.Constants.Constants.Formatters;

namespace DoggetTelegramBot.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class Permissions
        {
            public static Error UsageTime(TimeSpan duration, DateTime allowedAccessDate) => Error.Forbidden(
                code: "Permission.UsageTime",
                description: $"You can only use this command once per {Formatters.FormatCommandUsageTimeIntoString(duration)}. You are allowed to use after {allowedAccessDate}.");
        }
    }
}
