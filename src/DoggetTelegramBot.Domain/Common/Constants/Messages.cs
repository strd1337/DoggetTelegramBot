namespace DoggetTelegramBot.Domain.Common.Constants
{
    public static partial class Constants
    {
        public static partial class Messages
        {
            public const string TimeExpired = "Unfortunately, time has expired.";
            public const string NotAllowed = "You are not allowed.";

            public static string NotFoundUserReply(string command) =>
                $"Select the user and reply on his message using the command: {command}";

            public static string NotFoundUserReply(string command, string key) =>
                $"Select the user and reply on his message using the command: {command} {key}";
        }
    }
}
