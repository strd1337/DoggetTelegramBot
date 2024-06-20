using DoggetTelegramBot.Domain.Common.Entities;

namespace DoggetTelegramBot.Domain.Models.FamilyEntity
{
    public sealed class Family : Root<FamilyId, Guid>
    {
        private readonly List<FamilyMember> members = [];

        public IReadOnlyList<FamilyMember> Members => members.AsReadOnly();

        private Family(FamilyId familyId) : base(familyId) { }

        public static Family Create() => new(FamilyId.CreateUnique());

        public Family()
        {
        }
    }
}
