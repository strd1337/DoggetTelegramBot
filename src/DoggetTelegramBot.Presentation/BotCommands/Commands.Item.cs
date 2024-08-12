namespace DoggetTelegramBot.Presentation.BotCommands
{
    public static partial class Commands
    {
        public static partial class Item
        {
            public const string Purchase = $"{BotNickname} shop";
            public const string Add = $"shop +item";

            public static readonly CommandInfo PurchaseCommandInfo = new(
                Purchase,
                "Opens the shop to purchase items.");

            public static readonly CommandInfo AddCommandInfo = new(
                Add,
                "Adds a new item to the shop.");
        }
    }
}
