using DoggetTelegramBot.Domain.Common.Entities;
using DoggetTelegramBot.Domain.Models.ItemEntity;

namespace DoggetTelegramBot.Domain.Models.InventoryEntity
{
    public sealed class Inventory : Root<InventoryId, Guid>
    {
        private readonly List<ItemId> itemIds = [];

        public decimal YuanBalance { get; set; }

        public IReadOnlyList<ItemId> ItemIds => itemIds.AsReadOnly();

        private Inventory(InventoryId inventoryId, decimal yuanBalance = 0) :
                base(inventoryId) => YuanBalance = yuanBalance;

        public static Inventory Create(decimal yuanBalance = 0)
            => new(InventoryId.CreateUnique(), yuanBalance);

        public Inventory()
        {
        }
    }
}
