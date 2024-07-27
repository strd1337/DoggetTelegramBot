using PRTelegramBot.Attributes;

namespace DoggetTelegramBot.Presentation.Handlers.Common.Enums
{
    [InlineCommand]
    public enum AddItemsStepCommands
    {
        SelectItemType = 1200,
        SelectItemAmountType,
    }
}
