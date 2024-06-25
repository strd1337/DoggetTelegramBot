namespace DoggetTelegramBot.Domain.Common.Constants
{
    public static partial class Constants
    {
        public const string ConnectionString = "DefaultConnection";
        private const string BotNickname = "dog";

        public static class Messages
        {
            public const string TimeExpired = "Time expired.";
            public const string NotAllowed = $"You are not allowed.";

            public static string NotFoundUserReply(string command) =>
                $"Select the user and reply on his message using the command: {command}";
        }
    }
}
