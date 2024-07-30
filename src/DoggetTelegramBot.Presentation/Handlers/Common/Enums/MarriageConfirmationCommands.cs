using System.ComponentModel;
using PRTelegramBot.Attributes;

namespace DoggetTelegramBot.Presentation.Handlers.Common.Enums
{
    [InlineCommand]
    public enum MarriageConfirmationCommands
    {
        [Description(nameof(Yes))]
        Yes = 500,
        [Description(nameof(No))]
        No,
    }
}
