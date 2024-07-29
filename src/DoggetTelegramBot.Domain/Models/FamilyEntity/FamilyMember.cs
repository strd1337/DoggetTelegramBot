using DoggetTelegramBot.Domain.Common.Entities;
using DoggetTelegramBot.Domain.Models.FamilyEntity.Enums;
using DoggetTelegramBot.Domain.Models.UserEntity;

namespace DoggetTelegramBot.Domain.Models.FamilyEntity
{
    public sealed class FamilyMember : Entity
    {
        public FamilyMemberId FamilyMemberId { get; private set; }
        public UserId UserId { get; private set; }
        public FamilyRole Role { get; private set; }

        private FamilyMember(
            FamilyMemberId familyMemberId,
            UserId userId,
            FamilyRole role)
        {
            FamilyMemberId = familyMemberId;
            UserId = userId;
            Role = role;
        }

        public static FamilyMember Create(UserId userId, FamilyRole role)
            => new(FamilyMemberId.CreateUnique(), userId, role);

#pragma warning disable CS8618
        private FamilyMember()
        {
        }
#pragma warning restore CS8618
    }
}
