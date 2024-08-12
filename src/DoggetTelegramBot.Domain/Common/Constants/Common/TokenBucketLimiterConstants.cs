namespace DoggetTelegramBot.Domain.Common.Constants.Common
{
    public static partial class Constants
    {
        public static partial class TokenBucketLimiter
        {
            public static class PolicyName
            {
                public const string PerChatLimit = "PerChatLimit";
                public const string GlobalLimit = "GlobalLimit";
                public const string PerGroupLimit = "PerGroupLimit";
            }
        }
    }
}
