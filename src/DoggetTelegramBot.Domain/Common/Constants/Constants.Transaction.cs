namespace DoggetTelegramBot.Domain.Common.Constants
{
    public static partial class Constants
    {
        public static class Transaction
        {
            public static class Messages
            {
                public static string ExecuteTransfer(bool isStarted = true) =>
                   isStarted ?
                   $"Execute transfer transaction request started." :
                   $"Execute transfer transaction request ended.";

                public static string ExecuteServiceFee(bool isStarted = true) =>
                   isStarted ?
                   $"Execute service fee transaction request started." :
                   $"Execute service fee transaction request ended.";

                public static string ExecutePurchase(bool isStarted = true) =>
                   isStarted ?
                   $"Execute purchase transaction request started." :
                   $"Execute purchase transaction request ended.";
            }
        }
    }
}
