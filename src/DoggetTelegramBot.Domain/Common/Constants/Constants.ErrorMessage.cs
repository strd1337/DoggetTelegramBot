namespace DoggetTelegramBot.Domain.Common.Constants
{
    public static partial class Constants
    {
        public static class ErrorMessage
        {
            public const string BotBlockedByUser = "Forbidden: bot was blocked by the user";
            public const string PrivacyRestricted = "BUTTON_USER_PRIVACY_RESTRICTED";
            public const string GroupChatUpgraded = "group chat was upgraded to a supergroup chat";

            public const string Conflict = "A conflict occurred. {0}";
            public const string BadRequest = "There was a problem with your request. {0}";
            public const string NotFound = "The requested resource was not found. {0}";
            public const string Unauthorized = "Unauthorized access.";
            public const string Forbidden = "Forbidden access.";
            public const string InternalServer = "An unexpected error occurred. Please try again later.";
            public const string Generic = "An error occurred. Please try again.";

            public const string NotAllowedFunction = "You are not allowed to use this function.";
            public const string MissingInformation = "Missing required information to check the data.";
        }
    }
}
