using System.ComponentModel;

namespace DoggetTelegramBot.Domain.Models.ItemEntity.Enums
{
    public enum ItemAmountType
    {
        [Description("10")]
        Ten = 10,
        [Description("20")]
        Twelve = 20,
        [Description("50")]
        Fifty = 50,
        [Description("100")]
        OneHundred = 100,
        [Description("200")]
        TwoHundred = 200,
    }
}
