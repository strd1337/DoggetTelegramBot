namespace DoggetTelegramBot.Presentation.BotCommands
{
    public static partial class Commands
    {
        public static partial class Family
        {
            public const string AddToFamily = "+family";
            public const string RemoveFromFamily = "-family";

            public static readonly CommandInfo AddToFamilyCommandInfo = new(
                AddToFamily,
                "Adds a user to the family.");

            public static readonly CommandInfo RemoveFromFamilyCommandInfo = new(
                RemoveFromFamily,
                "Removes a user from the family.");
        }
    }
}
