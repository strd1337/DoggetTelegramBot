using DoggetTelegramBot.Domain.Common.Entities;
using DoggetTelegramBot.Domain.Models.MarriageEntity.Enums;
using DoggetTelegramBot.Domain.Models.UserEntity;

namespace DoggetTelegramBot.Domain.Models.MarriageEntity
{
    public sealed class Marriage : Entity
    {
        private readonly List<UserId> spouseIds = [];

        public MarriageId MarriageId { get; private set; }
        public DateTime MarriageDate { get; private set; }
        public DateTime? DivorceDate { get; private set; }
        public MarriageType Type { get; private set; }
        public MarriageStatus Status { get; private set; }

        public IReadOnlyList<UserId> SpouseIds => spouseIds.AsReadOnly();

        private Marriage(
            MarriageId marriageId,
            DateTime marriageDate,
            MarriageType type,
            MarriageStatus status)
        {
            MarriageId = marriageId;
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

        public void AddSpouses(List<UserId> spouseIds) =>
            this.spouseIds.AddRange(spouseIds);

        public void Update(DateTime? divorceDate, MarriageStatus status)
        {
            DivorceDate = divorceDate;
            Status = status;
        }

#pragma warning disable CS8618
        private Marriage()
        {
        }
#pragma warning restore CS8618
    }
}
