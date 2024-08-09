using DoggetTelegramBot.Domain.Models.InventoryEntity;

namespace DoggetTelegramBot.Domain.Common.Constants.Inventory
{
    public static partial class Constants
    {
        public static partial class Inventory
        {
            public static class Logging
            {
                public static string Retrieved(InventoryId inventoryId) =>
                   $"Inventory {inventoryId.Value} was retrieved.";

                public static string NotFoundRetrieved(InventoryId inventoryId) =>
                    $"Inventory {inventoryId.Value} was not found.";

                public static string Created(Guid inventoryId) =>
                   $"Inventory {inventoryId} was created.";

                public static string UpdatedSuccessfully(InventoryId inventoryId) =>
                   $"Inventory {inventoryId.Value} was successfully updated.";
            }
        }
    }
}
