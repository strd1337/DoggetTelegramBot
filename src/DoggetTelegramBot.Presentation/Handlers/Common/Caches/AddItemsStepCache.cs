using DoggetTelegramBot.Domain.Models.ItemEntity.Enums;
using PRTelegramBot.Interfaces;

namespace DoggetTelegramBot.Presentation.Handlers.Common.Caches
{
    public sealed class AddItemsStepCache : ITelegramCache
    {
        private readonly List<string> serverNames = [];

        public ItemType Type { get; set; } = default!;
        public string ServerName { get; set; } = default!;
        public ItemAmountType AmountType { get; set; }
        public Dictionary<ItemAmountType, List<string>> Values { get; set; } = [];

        public IReadOnlyList<string> ServerNames => serverNames.AsReadOnly();

        public void AddServerNames(List<string> serverNames) =>
            this.serverNames.AddRange(serverNames);

        public bool ClearData()
        {
            ServerName = string.Empty;
            Values = [];
            Type = default;
            AmountType = default;
            return true;
        }
    }
}
