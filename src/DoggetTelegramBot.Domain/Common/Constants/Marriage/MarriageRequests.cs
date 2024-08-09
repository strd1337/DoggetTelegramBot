namespace DoggetTelegramBot.Domain.Common.Constants.Marriage
{
    public static partial class Constants
    {
        public static partial class Marriage
        {
            public static class Requests
            {
                public static string GetInformation(bool isStarted = true) =>
                    isStarted ?
                    $"Get marriage information request started." :
                    $"Get marriage information request ended.";

                public static string Marry(bool isStarted = true) =>
                    isStarted ?
                    $"Marry request started." :
                    $"Marry request ended.";

                public static string Divorce(bool isStarted = true) =>
                    isStarted ?
                    $"Divorce request started." :
                    $"Divorce request ended.";

                public static string Delete(bool isStarted = true) =>
                    isStarted ?
                    $"Delete marriage request started." :
                    $"Delete marriage request ended.";
            }
        }
    }
}
