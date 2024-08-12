using DoggetTelegramBot.Domain.Models.ItemEntity;

namespace DoggetTelegramBot.Domain.Common.Constants.Item
{
    public static partial class Constants
    {
        public static partial class Item
        {
            public static class Logging
            {
                public static string Updated(ItemId itemId) =>
                   $"Item {itemId.Value} was successfully updated.";

                public static string Created(List<ItemId> itemIds) =>
                    $"Items {string.Join(",", itemIds.Select(id => id.Value))} were created.";

                public static string RetrievedServerNames(bool isEmpty = false) =>
                    !isEmpty ?
                    $"Server names were retrieved." :
                    $"Server names were not retrieved.";
            }
        }
    }
}
