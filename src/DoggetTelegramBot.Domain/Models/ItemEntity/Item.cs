using DoggetTelegramBot.Domain.Common.Entities;
using DoggetTelegramBot.Domain.Models.ItemEntity.Enums;

namespace DoggetTelegramBot.Domain.Models.ItemEntity
{
    public sealed class Item : Entity
    {
        public ItemId ItemId { get; private set; }
        public string ServerName { get; private set; }
        public int Count { get; private set; }
        public ItemType Type { get; private set; }
        public ItemAmountType? AmountType { get; private set; }
        public decimal Price { get; private set; }
        public string Value { get; private set; }

        private Item(
            ItemId itemId,
            string serverName,
            ItemType type,
            decimal price,
            string value,
            ItemAmountType? amountType = null,
            int count = 1)
        {
            ItemId = itemId;
            ServerName = serverName;
            Count = count;
            Type = type;
            Price = price;
            Value = value;
            AmountType = amountType;
        }

        public static Item Create(
            string serverName,
            ItemType type,
            decimal price,
            string value,
            ItemAmountType? amountType = null,
            int count = 1) => new(
                ItemId.CreateUnique(),
                serverName,
                type,
                price,
                value,
                amountType,
                count);

        public void DiminishCount() => --Count;

        public void Delete() => IsDeleted = true;

#pragma warning disable CS8618
        private Item()
        {
        }
#pragma warning restore CS8618
    }
}
