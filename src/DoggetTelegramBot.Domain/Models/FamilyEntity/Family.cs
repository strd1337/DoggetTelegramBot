using DoggetTelegramBot.Domain.Common.Entities;

namespace DoggetTelegramBot.Domain.Models.FamilyEntity
{
    public sealed class Family : Entity
    {
        private readonly List<FamilyMember> members = [];

        public FamilyId FamilyId { get; private set; }

        public IReadOnlyList<FamilyMember> Members => members.AsReadOnly();

        private Family(FamilyId familyId) => FamilyId = familyId;

        public static Family Create() => new(FamilyId.CreateUnique());

#pragma warning disable CS8618
        private Family()
        {
        }
#pragma warning restore CS8618
    }
}
