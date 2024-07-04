using DoggetTelegramBot.Domain.Common.Entities;

namespace DoggetTelegramBot.Domain.Models.InventoryEntity
{
    public sealed class Inventory : Entity
    {
        public InventoryId InventoryId { get; private set; }
        public decimal YuanBalance { get; private set; }

        private Inventory(
            InventoryId inventoryId,
            decimal yuanBalance = 0)
        {
            InventoryId = inventoryId;
            YuanBalance = yuanBalance;
        }

        public static Inventory Create(decimal yuanBalance = 0)
            => new(InventoryId.CreateUnique(), yuanBalance);

        public bool HasSufficientBalance(decimal amount) =>
            YuanBalance >= amount;

        public void IncreaseBalance(decimal amount) =>
            YuanBalance += amount;

        public void DeductBalance(decimal amount) =>
            YuanBalance -= amount;

#pragma warning disable CS8618
        private Inventory()
        {
        }
#pragma warning restore CS8618
    }
}
