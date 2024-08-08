using DoggetTelegramBot.Domain.Models.TransactionEntity.Enums;

namespace DoggetTelegramBot.Domain.Common.Constants
{
    public static partial class Constants
    {
        public static class User
        {
            public static readonly TimeSpan UserMessageActivityTimeout = TimeSpan.FromMinutes(5);

            public const int MaxMessageCount = 10;

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

                public static string GetRequest(bool isStarted = true) =>
                    isStarted ?
                    $"Get user request started." :
                    $"Get user request ended.";

                public static string GetTransactionParticipantsRequest(bool isStarted = true) =>
                    isStarted ?
                    $"Get transaction participants request started." :
                    $"Get transaction participants request ended.";

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

                public static string ValidMessageRewardRequest(bool isStarted = true) =>
                    isStarted ?
                    $"Valid user message reward request started." :
                    $"Valid user message reward request ended.";

                public static string AddNewChatMemberRequest(bool isStarted = true) =>
                   isStarted ?
                   $"Add new chat member request started." :
                   $"Add new chat member request ended.";

                public static string ChatMemberLeftRequest(bool isStarted = true) =>
                   isStarted ?
                   $"Chat member lfet request started." :
                   $"Chat member left request ended.";

                public static string UpdateRequest(bool isStarted = true) =>
                    isStarted ?
                    $"Update user request started." :
                    $"Update user request ended.";

                public static string DeleteRequest(bool isStarted = true) =>
                    isStarted ?
                    $"Delete user request started." :
                    $"Delete user request ended.";

                public static string SendChoice(bool isAgreed = true) =>
                    isAgreed ?
                    "You have agreed." :
                    "You have not agreed";

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

                public static string NotFoundRetrieved(
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

                public static string RewardSentSuccessfully(decimal amount, RewardType rewardType)
                {
                    string reason = rewardType switch
                    {
                        RewardType.MessageCount => $"for sending {MaxMessageCount} messages.",
                        RewardType.ReactionToPost => "for reaction to a post.",
                        RewardType.NewChatMember => "for joining the chat.",
                        RewardType.MarriedUsersDaily => "for being married.",
                        _ => "for just like that."
                    };

                    return $"You have received {amount} yuan{(amount > 1 ? "s" : string.Empty)} {reason}";
                }
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
