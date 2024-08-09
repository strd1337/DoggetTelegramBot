using DoggetTelegramBot.Domain.Models.ItemEntity.Enums;
using PRTelegramBot.Extensions;
using System.Globalization;
using System.Text;

namespace DoggetTelegramBot.Domain.Common.Constants.Item
{
    public static partial class Constants
    {
        public static partial class Item
        {
            public static partial class Purchase
            {
                public static class Messages
                {
                    public static string SelectItemServerNameRequest(ItemType itemType) =>
                        $"You have selected {itemType.GetDescription()}. Please, select one of the server names:";

                    public static string ServerNamesNotFound =>
                        "Sorry, but there is no servers that have any items.";

                    public static string SuccessfulConfirmation(
                        ItemType type,
                        string serverName,
                        ItemAmountType? amountType = null)
                    {
                        StringBuilder sb = new();

                        sb.Append(string.Create(
                            CultureInfo.InvariantCulture,
                            $"You have selected {type.GetDescription().ToLower(CultureInfo.CurrentCulture)}"));

                        sb.Append(string.Create(
                            CultureInfo.InvariantCulture,
                            $" on {serverName}"));

                        if (amountType is not null)
                        {
                            sb.Append(string.Create(
                                CultureInfo.InvariantCulture,
                                $" with amount of {amountType.GetDescription()}"));
                        }

                        return sb.Append('.').ToString();
                    }
                }
            }
        }
    }
}
