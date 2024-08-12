namespace DoggetTelegramBot.Domain.Common.Constants.Inventory
{
    public static partial class Constants
    {
        public static partial class Inventory
        {
            public static class Requests
            {
                public static string GetInformation(bool isStarted = true) =>
                    isStarted ?
                    $"Get user inventory information request started." :
                    $"Get user inventory information request ended.";

                public static string Update(bool isStarted = true) =>
                    isStarted ?
                    $"Update user inventory request started." :
                    $"Update user inventory request ended.";

                public static string Transfer(bool isStarted = true) =>
                    isStarted ?
                    $"Transfer request started." :
                    $"Transfer request ended.";
            }
        }
    }
}
