using PRTelegramBot.Attributes;
using System.ComponentModel;

namespace DoggetTelegramBot.Presentation.Handlers.Common.Enums
{
    [InlineCommand]
    public enum SelectAmountTypeConfirmationCommands
    {
        [Description(nameof(Yes))]
        Yes = 510,
        [Description(nameof(No))]
        No,
    }
}
