using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Families.Common;
using DoggetTelegramBot.Domain.Models.FamilyEntity;
using ErrorOr;
using DoggetTelegramBot.Domain.Models.FamilyEntity.Enums;
using DoggetTelegramBot.Domain.Common.Errors;

namespace DoggetTelegramBot.Application.Families.Commands.Create
{
    public sealed class CreateFamilyCommandHandler(
        IUnitOfWork unitOfWork) : ICommandHandler<CreateFamilyCommand, FamilyResult>
    {
        public async Task<ErrorOr<FamilyResult>> Handle(
            CreateFamilyCommand request,
            CancellationToken cancellationToken)
        {
            var familyRepository = unitOfWork.GetRepository<Family, FamilyId>();

            var family = unitOfWork.GetRepository<Family, FamilyId>()
                .GetWhere(f => f.Members.Any(
                            m => request.SpouseIds.Contains(m.UserId) &&
                                m.IsPet(m.Role) &&
                                !m.IsDeleted) &&
                        !f.IsDeleted,
                    nameof(Family.Members))
                .FirstOrDefault();

            if (family is not null)
            {
                return Errors.Family.SomeoneIsPet;
            }

            List<FamilyMember> familyMembers = request.SpouseIds
                .Select(s => FamilyMember.Create(s, FamilyRole.Parent))
                .ToList();

            Family newFamily = Family.Create();
            newFamily.AddMembers(familyMembers);

            await familyRepository
                .AddAsync(newFamily, cancellationToken);

            return new FamilyResult(newFamily.FamilyId);
        }
    }
}
