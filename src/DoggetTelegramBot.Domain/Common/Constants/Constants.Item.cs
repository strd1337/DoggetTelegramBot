using System.Globalization;
using System.Text;
using DoggetTelegramBot.Domain.Models.ItemEntity;
using DoggetTelegramBot.Domain.Models.ItemEntity.Enums;
using PRTelegramBot.Extensions;

namespace DoggetTelegramBot.Domain.Common.Constants
{
    public static partial class Constants
    {
        public static class Item
        {
            public static readonly TimeSpan PurchaseUsageTime = TimeSpan.FromDays(1);
            public const int PurchaseTimeoutInSeconds = 60;

            public static class Messages
            {
                public static string GetRequest(bool isStarted = true) =>
                    isStarted ?
                    $"Get item request started." :
                    $"Get item request ended.";

                public static string PurchaseRequest(bool isStarted = true) =>
                    isStarted ?
                    $"Purchase an item request started." :
                    $"Purchase an item request ended.";

                public static string GetAllItemServerNamesRequest(bool isStarted = true) =>
                    isStarted ?
                    $"Get all item server names request started." :
                    $"Get all item server names request ended.";

                public static string RetrievedServerNames(bool isEmpty = false) =>
                    !isEmpty ?
                    $"Server names were retrieved." :
                    $"Server names were not retrieved.";

                public static string UpdatedSuccessfully(ItemId itemId) =>
                   $"Item {itemId.Value} was successfully updated.";

                public static class Purchase
                {
                    public static string SelectItemTypeRequest =>
                        "Wellcome, time for choosing is limited. Please, select one of the items:";

                    public static string SelectItemServerNameRequest(ItemType itemType) =>
                        $"You have selected {itemType.GetDescription()}. Please, select one of the server names:";

                    public static string SelectItemAmountTypeRequest(string serverName) =>
                        $"You have selected {serverName}. Please, select one of the amounts:";

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
            public static class ReplyKeys
            {
                public const string Purchase = $"{BotNickname} shop";
            }
        }
    }
}
