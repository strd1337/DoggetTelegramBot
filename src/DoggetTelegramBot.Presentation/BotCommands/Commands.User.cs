namespace DoggetTelegramBot.Presentation.BotCommands
{
    public static partial class Commands
    {
        public static partial class User
        {
            public const string GetMyInfo = $"{BotNickname} my stat";
            public const string UpdateNickname = $"{BotNickname} +nick";
            public const string DeleteNickname = $"{BotNickname} -nick";

            public static readonly CommandInfo GetMyInfoCommandInfo = new(
                GetMyInfo,
                "Gets the user's statistics.");

            public static readonly CommandInfo UpdateNicknameCommandInfo = new(
                UpdateNickname,
                "Updates the user's nickname.");

            public static readonly CommandInfo DeleteNicknameCommandInfo = new(
                DeleteNickname,
                "Deletes the user's nickname.");
        }
    }
}
