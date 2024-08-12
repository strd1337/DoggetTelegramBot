namespace DoggetTelegramBot.Domain.Common.Constants.User
{
    public static partial class Constants
    {
        public static partial class User
        {
            public static readonly TimeSpan UserMessageActivityTimeout = TimeSpan.FromMinutes(5);
            public const int MaxMessageCount = 10;
        }
    }
}
