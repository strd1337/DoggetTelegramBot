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

        public void AddMembers(List<FamilyMember> familyMembers) =>
            members.AddRange(familyMembers);

        public override void Delete()
        {
            IsDeleted = true;
            members.ForEach(m => m.Delete());
        }

#pragma warning disable CS8618
        private Family()
        {
        }
#pragma warning restore CS8618
    }
}
