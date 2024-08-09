using DoggetTelegramBot.Application.Common.CQRS;
using DoggetTelegramBot.Application.Common.Interfaces;
using DoggetTelegramBot.Application.Common.Services;
using DoggetTelegramBot.Application.Families.Common;
using DoggetTelegramBot.Domain.Common.Enums;
using DoggetTelegramBot.Domain.Common.Errors;
using DoggetTelegramBot.Domain.Models.FamilyEntity;
using ErrorOr;
using DoggetTelegramBot.Domain.Models.FamilyEntity.Enums;
using LoggerConstants = DoggetTelegramBot.Domain.Common.Constants.Logger.Constants.Logger;
using FamilyConstants = DoggetTelegramBot.Domain.Common.Constants.Family.Constants.Family;

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
                   FamilyConstants.Logging.NotFoundRetrieved(request.SpouseIds),
                   TelegramEvents.Message,
                   LoggerConstants.Colors.Get);

                return Errors.Family.NotFound;
            }

            logger.LogCommon(
                FamilyConstants.Logging.Retrieved(family.FamilyId),
                TelegramEvents.Message,
                LoggerConstants.Colors.Get);

            family.Delete();

            await familyRepository.UpdateAsync(family);

            return new FamilyResult(family.FamilyId);
        }
    }
}
