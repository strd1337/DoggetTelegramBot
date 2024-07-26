using PRTelegramBot.Attributes;

namespace DoggetTelegramBot.Presentation.Handlers.Common.Enums
{
    [InlineCommand]
    public enum BuyItemStepCommands
    {
        SelectItemType = 1000,
        SelectItemServerName,
        SelectItemAmountType,
    }
}
