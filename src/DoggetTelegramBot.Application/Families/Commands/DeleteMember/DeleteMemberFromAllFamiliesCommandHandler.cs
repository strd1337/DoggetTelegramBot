using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Families.Common;
using DoggetTelegramBot.Domain.Models.FamilyEntity;
using ErrorOr;
using DoggetTelegramBot.Domain.Models.FamilyEntity.Enums;
using DoggetTelegramBot.Domain.Common.Errors;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Common.Constants;

namespace DoggetTelegramBot.Application.Families.Commands.DeleteMember
{
    public sealed class DeleteMemberFromAllFamiliesCommandHandler(
        IUnitOfWork unitOfWork,
        IBotLogger logger) : ICommandHandler<DeleteMemberFromAllFamiliesCommand, DeleteMemberFromAllFamiliesResult>
    {
        public Task<ErrorOr<DeleteMemberFromAllFamiliesResult>> Handle(
            DeleteMemberFromAllFamiliesCommand request,
            CancellationToken cancellationToken)
        {
            var familyRepository = unitOfWork.GetRepository<Family, FamilyId>();

            List<Family> families =
            [
                .. familyRepository
                    .GetAll(nameof(Family.Members))
                    .Where(f => f.Members.Any(m => m.UserId == request.UserId &&
                            m.Role != FamilyRole.Parent &&
                            !m.IsDeleted) &&
                        !f.IsDeleted)
            ];

            if (families.Count == 0)
            {
                logger.LogCommon(
                    Constants.Family.Messages.NotFoundRetrieved(request.UserId),
                    TelegramEvents.GroupAction,
                    Constants.LogColors.GetAll);

                return Task.FromResult<ErrorOr<DeleteMemberFromAllFamiliesResult>>(
                    Errors.Family.NotFoundAny);
            }

            List<FamilyId> familyIds = families
                .Select(f => f.FamilyId)
                .ToList();

            logger.LogCommon(
                Constants.Family.Messages.Retrieved(familyIds),
                TelegramEvents.GroupAction,
                Constants.LogColors.GetAll);

            families.ForEach(family =>
            {
                var member = family.Members.First(m => m.UserId == request.UserId);

                member.Delete();
            });

            return Task.FromResult<ErrorOr<DeleteMemberFromAllFamiliesResult>>(
                new DeleteMemberFromAllFamiliesResult(familyIds));
        }
    }
}
