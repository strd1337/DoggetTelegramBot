using System.ComponentModel;

namespace DoggetTelegramBot.Domain.Models.ItemEntity.Enums
{
    public enum ItemAmountType
    {
        [Description("10")]
        Ten,
        [Description("20")]
        Twelve,
        [Description("50")]
        Fifty,
        [Description("100")]
        OneHundred,
        [Description("200")]
        TwoHundred,
    }
}
