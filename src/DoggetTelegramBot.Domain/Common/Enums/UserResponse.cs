using System.ComponentModel;
using PRTelegramBot.Attributes;

namespace DoggetTelegramBot.Domain.Common.Enums
{
    [InlineCommand]
    public enum UserResponse
    {
        [Description(nameof(Yes))]
        Yes = 500,
        [Description(nameof(No))]
        No,
    }
}
