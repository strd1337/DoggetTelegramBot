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
            public static partial class Add
            {
                public static class Messages
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
        }
    }
}
