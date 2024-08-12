namespace DoggetTelegramBot.Domain.Common.Constants.Transaction
{
    public static partial class Constants
    {
        public static partial class Transaction
        {
            public static class Requests
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

                public static string ExecuteRewardUser(bool isStarted = true) =>
                   isStarted ?
                   $"Execute user reward transaction request started." :
                   $"Execute user reward transaction request ended.";

                public static string ExecuteUserPenalty(bool isStarted = true) =>
                   isStarted ?
                   $"Execute user penalty transaction request started." :
                   $"Execute user penalty transaction request ended.";
            }
        }
    }
}
