using DoggetTelegramBot.Domain.Models.InventoryEntity;

namespace DoggetTelegramBot.Domain.Common.Constants
{
    public static partial class Constants
    {
        public static class Inventory
        {
            public static class Messages
            {
                public static string GetInformationRequest(bool isStarted = true) =>
                    isStarted ?
                    $"Get user inventory information request started." :
                    $"Get user inventory information request ended.";

                public static string UpdateRequest(bool isStarted = true) =>
                    isStarted ?
                    $"Update user inventory request started." :
                    $"Update user inventory request ended.";

                public static string TransferRequest(bool isStarted = true) =>
                    isStarted ?
                    $"Transfer request started." :
                    $"Transfer request ended.";

                public static string Created(Guid inventoryId) =>
                    $"Inventory {inventoryId} was created.";

                public static string UpdatedSuccessfully(InventoryId inventoryId) =>
                   $"Inventory {inventoryId.Value} was successfully updated.";

                public static string Retrieved(InventoryId inventoryId) =>
                    $"Inventory {inventoryId.Value} was retrieved.";

                public static string NotFoundRetrieved(InventoryId inventoryId) =>
                    $"Inventory {inventoryId.Value} was not found.";
            }

            public static class ReplyKeys
            {
                public const string GetInfo = $"{BotNickname} my inv";

                public const string Transfer = "+transfer";
                public const string TransferKey = "N, where N - amount of money.";
            }
        }
    }
}
