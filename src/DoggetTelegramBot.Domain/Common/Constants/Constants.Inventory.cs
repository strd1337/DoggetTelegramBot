namespace DoggetTelegramBot.Domain.Common.Constants
{
    public static partial class Constants
    {
        public static class Inventory
        {
            public static class Messages
            {
                public static string Created(Guid inventoryId) =>
                    $"Inventory {inventoryId} was created.";
            }
        }
    }
}
