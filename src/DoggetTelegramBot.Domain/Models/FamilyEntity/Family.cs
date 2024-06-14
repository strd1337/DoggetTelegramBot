using DoggetTelegramBot.Domain.Common.Entities;

namespace DoggetTelegramBot.Domain.Models.FamilyEntity
{
    public sealed class Family : Root<FamilyId, Guid>
    {
        private readonly List<FamilyMember> familyMembers = [];

        public IReadOnlyList<FamilyMember> FamilyMembers => familyMembers.AsReadOnly();

        private Family(FamilyId familyId) : base(familyId) { }

        public static Family Create() => new(FamilyId.CreateUnique());

        public Family()
        {
        }
    }
}
