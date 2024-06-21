using DoggetTelegramBot.Domain.Common.Entities;
using DoggetTelegramBot.Domain.Models.InventoryEntity;
using DoggetTelegramBot.Domain.Models.UserEntity.Enums;

namespace DoggetTelegramBot.Domain.Models.UserEntity
{
    public sealed class User : Root<UserId, Guid>
    {
        private readonly List<UserPrivilege> privileges = [];

        public long TelegramId { get; private set; }
        public string? Username { get; private set; }
        public string? Nickname { get; private set; }
        public DateTime RegisteredDate { get; private set; }
        public MaritalStatus MaritalStatus { get; private set; }
        public InventoryId InventoryId { get; private set; }

        public IReadOnlyList<UserPrivilege> Privileges
            => privileges.AsReadOnly();

        private User(
            UserId userId,
            long telegramId,
            string? username,
            DateTime registeredDate,
            InventoryId inventoryId,
            MaritalStatus maritalStatus) : base(userId)
        {
            TelegramId = telegramId;
            Username = username;
            RegisteredDate = registeredDate;
            InventoryId = inventoryId;
            MaritalStatus = maritalStatus;
        }

        public static User Create(
            long telegramId,
            string? username,
            DateTime registeredDate,
            InventoryId inventoryId,
            MaritalStatus maritalStatus) => new(
                UserId.CreateUnique(),
                telegramId,
                username,
                registeredDate,
                inventoryId,
                maritalStatus);

        public void AddPrivilege(UserPrivilege privilege)
            => privileges.Add(privilege);

        public void Update(string? nickname) => Nickname = nickname;

#pragma warning disable CS8618
        private User()
        {
        }
#pragma warning restore CS8618
    }
}
