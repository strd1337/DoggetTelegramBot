namespace DoggetTelegramBot.Domain.Common.Constants.User
{
    public static partial class Constants
    {
        public static partial class User
        {
            public static class Logging
            {
                public static string Registered(long telegramId) =>
                    $"User {telegramId} was registered.";

                public static string Retrieved(long telegramId) =>
                    $"User {telegramId} was retrieved.";

                public static string Retrieved(List<long> telegramIds) =>
                   $"Users {string.Join(",", telegramIds.Select(id => id))} were retrieved.";

                public static string NotFoundRetrieved(long telegramId) =>
                    $"User {telegramId} was not found.";

                public static string NotFoundRetrieved(List<long> telegramIds) =>
                    $"Users {string.Join(",", telegramIds.Select(id => id))} were not retrieved.";

                public static string NotFoundRetrieved(List<long> telegramIds, string cause) =>
                    $"Users {string.Join(",", telegramIds.Select(id => id))} were not retrieved. {cause}";


                public static string AccessedSuccessfully(long telegramId) =>
                    $"User {telegramId} was successfully granted.";

                public static string FailedAccess(long telegramId) =>
                    $"User {telegramId} was not granted.";

                public static string SuccessExistence(long telegramId) =>
                    $"User {telegramId} exists.";

                public static string UpdatedSuccessfully(long telegramId) =>
                   $"User {telegramId} was successfully updated.";

                public static string UpdatedSuccessfully(List<long> telegramIds) =>
                   $"Users {string.Join(",", telegramIds.Select(id => id))} were successfully updated.";
            }
        }
    }
}
