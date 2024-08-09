namespace DoggetTelegramBot.Domain.Common.Constants.Dungeon.Guimu
{
    public static partial class Constants
    {
        public static partial class Dungeon
        {
            public static partial class Guimu
            {
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
            }
        }
    }
}
