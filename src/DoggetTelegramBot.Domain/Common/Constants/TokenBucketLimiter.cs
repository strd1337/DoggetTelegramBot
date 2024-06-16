namespace DoggetTelegramBot.Domain.Common.Constants
{
    public static partial class Constants
    {
        public static class TokenBucketLimiter
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
