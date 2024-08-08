using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Families.Common;
using DoggetTelegramBot.Domain.Common.Constants;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Common.Errors;
using DoggetTelegramBot.Domain.Models.FamilyEntity;
using ErrorOr;
using DoggetTelegramBot.Domain.Models.FamilyEntity.Enums;

namespace DoggetTelegramBot.Application.Families.Commands.Delete
{
    public sealed class DeleteFamilyBySpouseIdsCommandHandler(
        IUnitOfWork unitOfWork,
        IBotLogger logger) : ICommandHandler<DeleteFamilyBySpouseIdsCommand, FamilyResult>
    {
        public async Task<ErrorOr<FamilyResult>> Handle(
            DeleteFamilyBySpouseIdsCommand request,
            CancellationToken cancellationToken)
        {
            var familyRepository = unitOfWork.GetRepository<Family, FamilyId>();

            var family = unitOfWork.GetRepository<Family, FamilyId>()
                .GetWhere(f => f.Members.Any(m => request.SpouseIds.Contains(m.UserId) &&
                        m.Role == FamilyRole.Parent &&
                        !m.IsDeleted) && !f.IsDeleted,
                    nameof(Family.Members))
                .FirstOrDefault();

            if (family is null)
            {
                logger.LogCommon(
                   Constants.Family.Messages.NotFoundRetrieved(request.SpouseIds),
                   TelegramEvents.Message,
                   Constants.LogColors.Get);

                return Errors.Family.NotFound;
            }

            logger.LogCommon(
                Constants.Family.Messages.Retrieved(family.FamilyId),
                TelegramEvents.Message,
                Constants.LogColors.Get);

            family.Delete();

            await familyRepository.UpdateAsync(family);

            return new FamilyResult(family.FamilyId);
        }
    }
}
