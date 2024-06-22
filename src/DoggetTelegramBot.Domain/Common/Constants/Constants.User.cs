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

                public static string GetSpousesRequest(bool isStarted = true) =>
                    isStarted ?
                    $"Get spouses request started." :
                    $"Get spouses request ended.";

                public static string UpdateNicknameRequest(bool isStarted = true) =>
                    isStarted ?
                    $"Update nickname request started." :
                    $"Update nickname request ended.";

                public static string UpdateMaritalStatusRequest(bool isStarted = true) =>
                    isStarted ?
                    $"Update marital status request started." :
                    $"Update marital status request ended.";

                public static string Registered(long telegramId) =>
                    $"User {telegramId} was registered.";

                public static string Retrieved(long telegramId) =>
                    $"User {telegramId} was retrieved.";

                public static string Retrieved(List<long> telegramIds) =>
                   $"Users {string.Join(",", telegramIds.Select(id => id))} were retrieved.";

                public static string NotFoundRetrieved(long telegramId) =>
                    $"User {telegramId} was not found.";

                public static string NotFoundRetrievedWithCause(
                    List<long> telegramIds,
                    string cause) =>
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

            public static class ReplyKeys
            {
                public const string GetMyInfo = $"{BotNickname} my stat";
                public const string UpdateNickname = $"{BotNickname} +nick";
                public const string DeleteNickname = $"{BotNickname} -nick";
            }
        }
    }
}
