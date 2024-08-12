namespace DoggetTelegramBot.Presentation.BotCommands
{
    public static partial class Commands
    {
        public static partial class Marriage
        {
            public const string Marry = "+marriage";
            public const string Divorce = "+divorce";

            public static readonly CommandInfo MarryCommandInfo = new(
                Marry,
                "Marries users.");

            public static readonly CommandInfo DivorceCommandInfo = new(
                Divorce,
                "Divorces users.");
        }
    }
}
