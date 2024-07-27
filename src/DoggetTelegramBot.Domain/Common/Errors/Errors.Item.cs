using ErrorOr;

namespace DoggetTelegramBot.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class Item
        {
            public static Error NotFound => Error.NotFound(
                code: "Item.NotFound",
                description: "Sorry, but there is no item requested.");

        }
    }
}
