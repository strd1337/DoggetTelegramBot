using DoggetTelegramBot.Domain.Models.TransactionEntity.Enums;

namespace DoggetTelegramBot.Domain.Common.Constants.User
{
    public static partial class Constants
    {
        public static partial class User
        {
            public static class Messages
            {
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
        }
    }
}
