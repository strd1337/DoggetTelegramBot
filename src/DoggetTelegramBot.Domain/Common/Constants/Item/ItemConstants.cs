namespace DoggetTelegramBot.Domain.Common.Constants.Item
{
    public static partial class Constants
    {
        public static partial class Item
        {
            public static readonly TimeSpan PurchaseUsageTime = TimeSpan.FromDays(1);
            public const int PurchaseTimeoutInSeconds = 60;
            public const int AddTimeoutInSeconds = 300;
        }
    }
}
