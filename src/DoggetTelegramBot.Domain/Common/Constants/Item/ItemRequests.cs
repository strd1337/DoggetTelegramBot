namespace DoggetTelegramBot.Domain.Common.Constants.Item
{
    public static partial class Constants
    {
        public static partial class Item
        {
            public static class Requests
            {
                public static string Get(bool isStarted = true) =>
                    isStarted ?
                    $"Get item request started." :
                    $"Get item request ended.";

                public static string GetAllItemServerNames(bool isStarted = true) =>
                    isStarted ?
                    $"Get all item server names request started." :
                    $"Get all item server names request ended.";

                public static string Purchase(bool isStarted = true) =>
                    isStarted ?
                    $"Purchase an item request started." :
                    $"Purchase an item request ended.";

                public static string Add(bool isStarted = true) =>
                    isStarted ?
                    $"Add items request started." :
                    $"Add items request ended.";
            }
        }
    }
}
