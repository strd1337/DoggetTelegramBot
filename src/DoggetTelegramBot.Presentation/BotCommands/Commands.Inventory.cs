namespace DoggetTelegramBot.Presentation.BotCommands
{
    public static partial class Commands
    {
        public static partial class Inventory
        {
            public const string GetInfo = $"{BotNickname} my inv";
            public const string Transfer = "+transfer";
            public const string TransferKey = "N, where N - amount of money.";
        }
    }
}
