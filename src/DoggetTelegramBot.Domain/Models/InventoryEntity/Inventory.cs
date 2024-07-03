using DoggetTelegramBot.Domain.Common.Entities;
using DoggetTelegramBot.Domain.Models.ItemEntity;

namespace DoggetTelegramBot.Domain.Models.InventoryEntity
{
    public sealed class Inventory : Entity
    {
        private readonly List<ItemId> itemIds = [];

        public InventoryId InventoryId { get; private set; }
        public decimal YuanBalance { get; private set; }

        public IReadOnlyList<ItemId> ItemIds => itemIds.AsReadOnly();

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

        public bool HasSufficientItems(List<ItemId> itemIds) =>
            itemIds.All(i => this.itemIds.Any(id => id.Value == i.Value));

        public void IncreaseBalance(decimal amount) =>
            YuanBalance += amount;

        public void DeductBalance(decimal amount) =>
            YuanBalance -= amount;

        public void AddItems(List<ItemId> items) =>
            itemIds.AddRange(items);

        public void DeductItems(List<ItemId> items)
        {
            foreach (var item in items)
            {
                itemIds.Remove(item);
            }
        }

#pragma warning disable CS8618
        private Inventory()
        {
        }
#pragma warning restore CS8618
    }
}
