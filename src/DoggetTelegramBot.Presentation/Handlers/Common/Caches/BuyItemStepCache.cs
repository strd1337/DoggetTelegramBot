using DoggetTelegramBot.Domain.Models.ItemEntity.Enums;
using PRTelegramBot.Interfaces;

namespace DoggetTelegramBot.Presentation.Handlers.Common.Caches
{
    public sealed class BuyItemStepCache : ITelegramCache
    {
        private readonly List<string> serverNames = [];

        public ItemType Type { get; set; } = default!;
        public string ServerName { get; set; } = default!;
        public ItemAmountType? AmountType { get; set; }
        public int Count { get; set; } = 1;

        public IReadOnlyList<string> ServerNames => serverNames.AsReadOnly();

        public void AddServerNames(List<string> serverNames) =>
            this.serverNames.AddRange(serverNames);

        public bool ClearData()
        {
            ServerName = string.Empty;
            return true;
        }
    }
}
