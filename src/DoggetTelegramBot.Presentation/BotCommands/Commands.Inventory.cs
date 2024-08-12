namespace DoggetTelegramBot.Presentation.BotCommands
{
    public static partial class Commands
    {
        public static partial class Inventory
        {
            public const string GetInfo = $"{BotNickname} my inv";
            public const string Transfer = $"+transfer {TransferKey}";
            public const string TransferKey = "N";

            public static readonly CommandInfo GetInfoCommandInfo = new(
                GetInfo,
                "Gets the inventory information.");

            public static readonly CommandInfo TransferCommandInfo = new(
                Transfer,
                $"Transfers money to another user, where {TransferKey} - amount of money.");
        }
    }
}
