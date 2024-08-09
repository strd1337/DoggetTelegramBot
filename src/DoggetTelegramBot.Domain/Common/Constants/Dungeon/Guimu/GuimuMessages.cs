namespace DoggetTelegramBot.Domain.Common.Constants.Dungeon.Guimu
{
    public static partial class Constants
    {
        public static partial class Dungeon
        {
            public static partial class Guimu
            {
                public static class Messages
                {
                    public static string Start(bool isStarted = true) =>
                        isStarted ?
                        "The Guimu dungeon game has started." :
                        "The Guimu dungeon game has ended.";
                }
            }
        }
    }
}
