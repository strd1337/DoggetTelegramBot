using DoggetTelegramBot.Domain.Common.Entities;
using DoggetTelegramBot.Domain.Models.ItemEntity.Enums;

namespace DoggetTelegramBot.Domain.Models.ItemEntity
{
    public sealed class Item : Root<ItemId, Guid>
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public ItemType Type { get; private set; }
        public decimal Price { get; private set; }

        private Item(
            ItemId itemId,
            string name,
            string description,
            ItemType type,
            decimal price) : base(itemId)
        {
            Name = name;
            Description = description;
            Type = type;
            Price = price;
        }

        public static Item Create(
            string name,
            string description,
            ItemType type,
            decimal price) => new(
                ItemId.CreateUnique(),
                name,
                description,
                type,
                price);

#pragma warning disable CS8618
        private Item()
        {
        }
#pragma warning restore CS8618
    }
}
