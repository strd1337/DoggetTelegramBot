namespace DoggetTelegramBot.Presentation.BotCommands
{
    public static partial class Commands
    {
        public static partial class User
        {
            public const string GetMyInfo = $"{BotNickname} my stat";
            public const string UpdateNickname = $"{BotNickname} +nick";
            public const string DeleteNickname = $"{BotNickname} -nick";
        }
    }
}
