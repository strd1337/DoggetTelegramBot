using System.Globalization;
using System.Text;
using DoggetTelegramBot.Domain.Models.InventoryEntity;
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
            public const int AddTimeoutInSeconds = 300;

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

                public static string AddRequest(bool isStarted = true) =>
                    isStarted ?
                    $"Add items request started." :
                    $"Add items request ended.";

                public static string GetAllItemServerNamesRequest(bool isStarted = true) =>
                    isStarted ?
                    $"Get all item server names request started." :
                    $"Get all item server names request ended.";

                public static string RetrievedServerNames(bool isEmpty = false) =>
                    !isEmpty ?
                    $"Server names were retrieved." :
                    $"Server names were not retrieved.";

                public static string Updated(ItemId itemId) =>
                   $"Item {itemId.Value} was successfully updated.";

                public static string Created(List<ItemId> itemIds) =>
                    $"Items {string.Join(",", itemIds.Select(id => id.Value))} were created.";

                public static string SelectItemTypeRequest(bool isPurchase = true) =>
                    $"{Constants.Messages.FormatTimeIntoString(isPurchase ? PurchaseTimeoutInSeconds : AddTimeoutInSeconds)} Please, select one of the items:";

                public static string SelectItemAmountTypeRequest(string serverName) =>
                    $"You have selected {serverName}. Please, select one of the amounts:";

                public static class Purchase
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

                public static class Add
                {
                    public static string SelectItemServerNameRequest(ItemType itemType) =>
                        $"You have selected {itemType.GetDescription()}. Please, select one of the following server names or write yours.";

                    public static string WriteValuesForAmountTypeRequest(ItemAmountType amountType) =>
                        $"You have selected the amount of {amountType.GetDescription()}. Please, write values for it.";

                    public static string SelectMoreAmountTypesRequest =>
                        $"All data are saved. Do you want to select other amounts?";

                    public static string SelectMoreAmountTypes =>
                        $"Great! You can choose other amounts. What is your choice?";

                    public static string SuccessfulConfirmation(
                        ItemType type,
                        string serverName,
                        Dictionary<ItemAmountType, List<string>> amountTypesWithValues)
                    {
                        StringBuilder sb = new();
                        sb.AppendLine($"Congratulations! Here is your data.\n");

                        sb.AppendLine(string.Create(CultureInfo.InvariantCulture, $"Item type: {type.GetDescription()}"));
                        sb.AppendLine(string.Create(CultureInfo.InvariantCulture, $"Server name: {serverName}\n"));

                        foreach (var amountType in amountTypesWithValues.Keys)
                        {
                            sb.AppendLine(string.Create(CultureInfo.InvariantCulture, $"Amount type: {amountType.GetDescription()}"));

                            sb.AppendLine(string.Create(
                                CultureInfo.InvariantCulture,
                                $"Values: {string.Join(", ", amountTypesWithValues[amountType].Select(value => value))}"));
                        }

                        return sb.ToString();
                    }
                }
            }

            public static class ReplyKeys
            {
                public const string Purchase = $"{BotNickname} shop";
                public const string Add = $"shop +item";
            }
        }
    }
}
