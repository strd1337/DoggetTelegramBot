namespace DoggetTelegramBot.Domain.Common.Constants
{
    public static partial class Constants
    {
        public static class User
        {
            public static class Messages
            {
                public const string StartCheckingPrivilege =
                "Check user privilege request started.";

                public const string StartCheckingExistence =
                    "Check user existence request started.";

                public static string Registered(long telegramId) =>
                    $"User {telegramId} was registered.";

                public static string Retrieved(long telegramId) =>
                    $"User {telegramId} was retrieved.";

                public static string NotFoundRetrieved(long telegramId) =>
                    $"User {telegramId} was not found.";

                public static string SuccessAccess(long telegramId) =>
                    $"User {telegramId} was granted successfully.";

                public static string FailedAccess(long telegramId) =>
                    $"User {telegramId} was not granted.";

                public static string SuccessExistence(long telegramId) =>
                    $"User {telegramId} exists.";
            }
        }
    }
}
