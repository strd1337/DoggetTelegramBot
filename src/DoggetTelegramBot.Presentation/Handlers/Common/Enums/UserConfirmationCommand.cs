using System.ComponentModel;
using PRTelegramBot.Attributes;

namespace DoggetTelegramBot.Presentation.Handlers.Common.Enums
{
    [InlineCommand]
    public enum UserConfirmationCommand
    {
        [Description(nameof(Yes))]
        Yes = 500,
        [Description(nameof(No))]
        No,
    }
}
