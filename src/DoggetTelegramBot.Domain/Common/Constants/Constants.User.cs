namespace DoggetTelegramBot.Domain.Common.Constants
{
    public static partial class Constants
    {
        public static class User
        {
            public static class Messages
            {
                public static string CheckPrivilegeRequest(bool isStarted = true) =>
                    isStarted ?
                    $"Check user privilege request started." :
                    $"Check user privilege request ended.";

                public static string CheckExistenceRequest(bool isStarted = true) =>
                    isStarted ?
                    $"Check user existence request started." :
                    $"Check user existence request ended.";

                public static string GetInformationRequest(bool isStarted = true) =>
                    isStarted ?
                    $"Get user information request started." :
                    $"Get user information request ended.";

                public static string UpdateNicknameRequest(bool isStarted = true) =>
                    isStarted ?
                    $"Update nickname request started." :
                    $"Update nickname request ended.";

                public static string Registered(long telegramId) =>
                    $"User {telegramId} was registered.";

                public static string Retrieved(long telegramId) =>
                    $"User {telegramId} was retrieved.";

                public static string NotFoundRetrieved(long telegramId) =>
                    $"User {telegramId} was not found.";

                public static string AccessedSuccessfully(long telegramId) =>
                    $"User {telegramId} was successfully granted.";

                public static string FailedAccess(long telegramId) =>
                    $"User {telegramId} was not granted.";

                public static string SuccessExistence(long telegramId) =>
                    $"User {telegramId} exists.";

                public static string UpdatedSuccessfully(long telegramId) =>
                   $"User {telegramId} was successfully updated.";
            }

            public static class ReplyKeys
            {
                public const string GetMyInfo = "who am i";
                public const string UpdateNickname = "+nick";
                public const string DeleteNickname = "-nick";
            }
        }
    }
}
