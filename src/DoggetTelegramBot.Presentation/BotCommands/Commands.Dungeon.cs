namespace DoggetTelegramBot.Presentation.BotCommands
{
    public static partial class Commands
    {
        public static partial class Dungeon
        {
            public const string Guimu = "/guimu";

            public static readonly CommandInfo GuimuCommandInfo = new(
                Guimu,
                "This command triggers the Guimu dungeon.");
        }
    }
}
