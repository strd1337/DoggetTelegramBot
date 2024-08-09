using Formatters = DoggetTelegramBot.Domain.Common.Constants.Constants.Formatters;

namespace DoggetTelegramBot.Domain.Common.Constants.Item
{
    public static partial class Constants
    {
        public static partial class Item
        {
            public static class Messages
            {
                public static string SelectItemTypeRequest(bool isPurchase = true) =>
                    $"{Formatters.FormatChoosingTimeIntoString(
                        isPurchase ?
                        PurchaseTimeoutInSeconds :
                        AddTimeoutInSeconds)} Please, select one of the items:";

                public static string SelectItemAmountTypeRequest(string serverName) =>
                       $"You have selected {serverName}. Please, select one of the amounts:";
            }
        }
    }
}
