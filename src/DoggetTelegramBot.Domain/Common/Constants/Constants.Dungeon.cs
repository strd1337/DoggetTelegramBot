namespace DoggetTelegramBot.Domain.Common.Constants
{
    public static partial class Constants
    {
        public static class Dungeon
        {
            public static class Guimu
            {
                public static readonly TimeSpan StartCommandUsageTime = TimeSpan.FromHours(4);

                public static class Rules
                {
                    public const int ZeroChanceThreshold = 15;
                    public const int LoseChanceThreshold = 30;
                    public const int SpecialCaseChanceThreshold = 35;

                    public const int MinLossAmount = 5;
                    public const int MaxLossAmount = 18;

                    public const int MinGainAmount = 5;
                    public const int MaxGainAmount = 38;

                    public const int SpecialGainAmount = 50;
                }

                public static class Messages
                {
                    public static string Start(bool isStarted = true) =>
                        isStarted ?
                        "The Guimu dungeon game has started." :
                        "The Guimu dungeon game has ended.";
                }
            }

            public static class ReplyKeys
            {
                public const string Guimu = "/guimu";
            }
        }
    }
}
