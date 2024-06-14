using DoggetTelegramBot.Domain.Common.Entities;
using DoggetTelegramBot.Domain.Models.InventoryEntity;
using DoggetTelegramBot.Domain.Models.UserEntity.Enums;

namespace DoggetTelegramBot.Domain.Models.UserEntity
{
    public sealed class User : Root<UserId, Guid>
    {
        public int TelegramId { get; private set; }
        public string Nickname { get; private set; }
        public InventoryId InventoryId { get; private set; }
        public MaritalStatus MaritalStatus { get; private set; }

        private User(
            UserId userId,
            int telegramId,
            string nickname,
            InventoryId inventoryId,
            MaritalStatus maritalStatus) : base(userId)
        {
            TelegramId = telegramId;
            Nickname = nickname;
            InventoryId = inventoryId;
            MaritalStatus = maritalStatus;
        }

        public static User Create(
            int telegramId,
            string nickname,
            InventoryId inventoryId,
            MaritalStatus maritalStatus) => new(
                UserId.CreateUnique(),
                telegramId,
                nickname,
                inventoryId,
                maritalStatus);

#pragma warning disable CS8618
        private User()
        {
        }
#pragma warning restore CS8618
    }
}
