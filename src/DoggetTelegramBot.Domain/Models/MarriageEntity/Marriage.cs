using DoggetTelegramBot.Domain.Common.Entities;
using DoggetTelegramBot.Domain.Models.MarriageEntity.Enums;
using DoggetTelegramBot.Domain.Models.UserEntity;

namespace DoggetTelegramBot.Domain.Models.MarriageEntity
{
    public sealed class Marriage : Root<MarriageId, Guid>
    {
        private readonly List<UserId> spouseIds = [];

        public DateTime MarriageDate { get; private set; }
        public DateTime? DivorceDate { get; private set; }
        public MarriageType Type { get; private set; }
        public MarriageStatus Status { get; private set; }

        public IReadOnlyList<UserId> SpouseIds => spouseIds.AsReadOnly();

        private Marriage(
            MarriageId marriageId,
            DateTime marriageDate,
            MarriageType type,
            MarriageStatus status) : base(marriageId)
        {
            MarriageDate = marriageDate;
            Type = type;
            Status = status;
        }

        public static Marriage Create(
            DateTime marriageDate,
            MarriageType type,
            MarriageStatus status) => new(
                MarriageId.CreateUnique(),
                marriageDate,
                type,
                status);

        public Marriage()
        {
        }
    }
}
