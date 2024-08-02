using DoggetTelegramBot.Domain.Common.Entities;
using DoggetTelegramBot.Domain.Models.InventoryEntity;
using DoggetTelegramBot.Domain.Models.UserEntity.Enums;

namespace DoggetTelegramBot.Domain.Models.UserEntity
{
    public sealed class User : Entity
    {
        private readonly List<UserPrivilege> privileges = [];

        public UserId UserId { get; private set; }
        public long TelegramId { get; private set; }
        public string? Username { get; private set; }
        public string? Nickname { get; private set; }
        public string FirstName { get; private set; }
        public DateTime RegisteredDate { get; private set; }
        public MaritalStatus MaritalStatus { get; private set; }
        public InventoryId InventoryId { get; private set; }

        public IReadOnlyList<UserPrivilege> Privileges
            => privileges.AsReadOnly();

        private User(
            UserId userId,
            long telegramId,
            string? username,
            string firstName,
            DateTime registeredDate,
            InventoryId inventoryId,
            MaritalStatus maritalStatus)
        {
            UserId = userId;
            TelegramId = telegramId;
            Username = username;
            FirstName = firstName;
            RegisteredDate = registeredDate;
            InventoryId = inventoryId;
            MaritalStatus = maritalStatus;
        }

        public static User Create(
            long telegramId,
            string? username,
            string firstName,
            DateTime registeredDate,
            InventoryId inventoryId,
            MaritalStatus maritalStatus) => new(
                UserId.CreateUnique(),
                telegramId,
                username,
                firstName,
                registeredDate,
                inventoryId,
                maritalStatus);

        public void AddPrivilege(UserPrivilege privilege)
            => privileges.Add(privilege);

        public void ClearPrivileges()
            => privileges.Clear();

        public void UpdateNickname(string? nickname) =>
            Nickname = nickname;

        public void UpdateMaritalStatus(MaritalStatus maritalStatus) =>
            MaritalStatus = maritalStatus;

        public void UpdateDetails(string? username, string firstName)
        {
            Username = username;
            FirstName = firstName;
        }

#pragma warning disable CS8618
        private User()
        {
        }
#pragma warning restore CS8618
    }
}
