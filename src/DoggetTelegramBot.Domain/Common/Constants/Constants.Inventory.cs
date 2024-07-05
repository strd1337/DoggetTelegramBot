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

                public static string Created(Guid inventoryId) =>
                    $"Inventory {inventoryId} was created.";
            }

            public static class ReplyKeys
            {
                public const string GetInfo = $"{BotNickname} my inv";
            }
        }
    }
}
