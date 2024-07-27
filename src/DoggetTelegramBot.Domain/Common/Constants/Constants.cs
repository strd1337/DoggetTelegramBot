namespace DoggetTelegramBot.Domain.Common.Constants
{
    public static partial class Constants
    {
        public const string ConnectionString = "DefaultConnection";
        private const string BotNickname = "dog";

        public static class Messages
        {
            public const string TimeExpired = "Unfortunately, time expired.";
            public const string NotAllowed = $"You are not allowed.";

            public static string NotFoundUserReply(string command) =>
                $"Select the user and reply on his message using the command: {command}";

            public static string NotFoundUserReply(string command, string key) =>
                $"Select the user and reply on his message using the command: {command} {key}";

            public static string FormatTimeIntoString(int timeoutInSeconds = 10)
            {
                TimeSpan timeout = TimeSpan.FromSeconds(timeoutInSeconds);
                int minutes = timeout.Minutes;
                int seconds = timeout.Seconds;

                string timeMessage = minutes > 0 ?
                    $"{minutes} minute{(minutes > 1 ? "s" : "")}" :
                    string.Empty;

                if (seconds > 0)
                {
                    if (!string.IsNullOrEmpty(timeMessage))
                    {
                        timeMessage += " and ";
                    }
                    timeMessage += $"{seconds} second{(seconds > 1 ? "s" : "")}";
                }

                return $"The time for choosing is limited to {timeMessage}.";
            }
        }
    }
}
