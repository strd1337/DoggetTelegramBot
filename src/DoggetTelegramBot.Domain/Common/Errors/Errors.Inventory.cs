using ErrorOr;

namespace DoggetTelegramBot.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class Inventory
        {
            public static Error SomeNotFound => Error.NotFound(
                code: "Inventories.SomeNotFound",
                description: "Some of the provided inventories were not found.");

            public static Error NotFound => Error.NotFound(
               code: "Inventory.NotFound",
               description: "Inventory is not found.");

            public static Error InsufficientBalance => Error.Validation(
              code: "Inventory.InsufficientBalance",
              description: "User has insufficient balance. Please check your inventory.");

            public static Error InsufficientInventories => Error.Validation(
              code: "Inventories.InsufficientInventories",
              description: "Some of the provided users have insufficient inventory. " +
                "Please check yours inventories.");
        }
    }
}
